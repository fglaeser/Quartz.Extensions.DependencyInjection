using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Quartz.Extensions.DependencyInjection
{
  public class SchedulerHostedService : IHostedService
  {
    readonly IJobFactory _jobFactory;
    readonly Type[] _job;
    readonly IConfiguration _configuration;
    private IScheduler _scheduler;

    public SchedulerHostedService(IJobFactory jobFactory, IConfiguration configuration, Type[] jobs)
    {
      _jobFactory = jobFactory;
      _configuration = configuration;
      _job = jobs;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {

      _scheduler = await StdSchedulerFactory.GetDefaultScheduler(cancellationToken);
      _scheduler.JobFactory = _jobFactory;

      //Add jobs
      for(int i=0; i < _job.Length; i++)
      {
        var jobType = _job[i];
        var jobName = jobType.Name;

        var options = new JobOptions();
        _configuration.Bind($"Quartz:Jobs:{jobName}", options);

        var dataMap = new JobDataMap();
        var dataSectionItems = _configuration.GetSection($"Quartz:Jobs:{jobName}:Data")?.GetChildren();

        foreach (IConfigurationSection item in dataSectionItems)
            dataMap[item.Key] = item.Value;        

        var job = JobBuilder.Create(jobType).WithIdentity(options.Identity).UsingJobData(dataMap).Build();

        var triggerBuilder = TriggerBuilder.Create()
            .WithIdentity($"{options.Identity}.trigger")     
            .WithCronSchedule(options.CronSchedule);
        if (options.StartNow) triggerBuilder.StartNow();

        await _scheduler.ScheduleJob(job, triggerBuilder.Build(), cancellationToken).ConfigureAwait(false);
      }

      await _scheduler.Start();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      return _scheduler.Shutdown(cancellationToken);
    }
  }
}
