using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Quartz.Spi;
using Quartz.Util;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Quartz.Extensions.DependencyInjection
{
  public class SchedulerHostedService : IHostedService
  {
    readonly IJobFactory _jobFactory;
    readonly QuartzOptions _options;
    readonly IConfiguration _configuration;
    readonly ISchedulerFactory _schedulerFactory;
    private IScheduler _scheduler;
      
    public SchedulerHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, IConfiguration configuration, IOptions<QuartzOptions> options)
    {
      _schedulerFactory = schedulerFactory;
      _jobFactory = jobFactory;
      _configuration = configuration;
      _options = options.Value;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
      _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
      _scheduler.JobFactory = _jobFactory;

      if (_options.EnsureDatabaseIsCreated) EnsureDatabaseCreated();
      //Add jobs
      foreach (var jobConfiguration in _options.Jobs)
      {
        var jobName = jobConfiguration.JobType.Name;
        var options = new JobOptions();

        if (jobConfiguration.Options == null)
          _configuration.Bind($"Quartz:Jobs:{jobName}", options);
        else
          options = jobConfiguration.Options;

        var dataMap = new JobDataMap();
        var dataSectionItems = _configuration.GetSection($"Quartz:Jobs:{jobName}:Data")?.GetChildren();

        foreach (IConfigurationSection item in dataSectionItems)
            dataMap[item.Key] = item.Value;        

        var job = JobBuilder.Create(jobConfiguration.JobType).WithIdentity(options.Identity).UsingJobData(dataMap).Build();
        if (!await _scheduler.CheckExists(job.Key, cancellationToken))
        {
          var triggerBuilder = TriggerBuilder.Create()
              .WithIdentity($"{options.Identity}.trigger")
              .WithCronSchedule(options.CronSchedule);
          if (options.StartNow) triggerBuilder.StartNow();
          await _scheduler.ScheduleJob(job, triggerBuilder.Build(), cancellationToken).ConfigureAwait(false);
        }
      }
      await _scheduler.Start();
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
      return _scheduler.Shutdown(_options.WaitForJobsToComplete, cancellationToken);
    }
    private void EnsureDatabaseCreated()
    {
      var conn = DBConnectionManager.Instance.GetConnection("myDs");
      conn.Open();

      try
      {
        Execute(conn, "SELECT count(*) FROM QRTZ_LOCKS");
      }
      catch (Exception e)
      {
        if (e.Message.Contains("no such table") || e.Message.Contains("doesn't exist"))
        {
          var connName = conn.GetType().FullName;

          switch (connName)
          {
            case "Microsoft.Data.Sqlite.SqliteConnection":
            { 
                using (var s = typeof(SchedulerHostedService).Assembly.GetManifestResourceStream("Quartz.Extensions.DependencyInjection.Database.tables_sqlite.sql"))
                using (var sr = new StreamReader(s))
                {
                  Execute(conn, sr.ReadToEnd());
                }
                break;
            }
            default:
              throw new ArgumentException($"Can't handle this DBConnection: {connName}");
          }
        }
      }
    }
    private void Execute(IDbConnection conn, string sql)
    {
      var command = conn.CreateCommand();
      command.CommandText = sql;
      command.ExecuteNonQuery();
    }

  }
}
