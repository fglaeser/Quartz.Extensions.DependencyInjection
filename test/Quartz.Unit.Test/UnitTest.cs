using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz.Extensions.DependencyInjection;
using Quartz.Impl.Triggers;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Quartz.Unit.Test
{
  public class UnitTest
  {
    public class MyJob : IJob
    {
      public Task Execute(IJobExecutionContext context)
      {
        return Task.CompletedTask;
      }
    }

    [Fact]
    public void inline_job_must_exist()
    {

      var host = new HostBuilder()
       .ConfigureServices(s =>
       {
         s.AddQuartz().AddJob<MyJob>(o =>
         {
           o.Identity = "My-Id";
           o.CronSchedule = Cron.EveryHour();
         });
       })
      .Build();

      host.Start();

      var scheduler = host.Services.GetRequiredService<ISchedulerFactory>().GetScheduler().GetAwaiter().GetResult();
      Assert.True(scheduler.CheckExists(new JobKey("My-Id")).GetAwaiter().GetResult());

      host.StopAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public void inline_job_cronschedule_test()
    {

      var host = new HostBuilder()
       .ConfigureServices(s =>
       {
         s.AddQuartz().AddJob<MyJob>(o =>
         {
           o.Identity = "My-Id";
           o.CronSchedule = Cron.EveryHour();
         });
       })
      .Build();

      host.Start();

      var scheduler = host.Services.GetRequiredService<ISchedulerFactory>().GetScheduler().GetAwaiter().GetResult();
      var trigger = scheduler.GetTriggersOfJob(new JobKey("My-Id")).GetAwaiter().GetResult().First();
      Assert.Equal(Cron.EveryHour(), ((CronTriggerImpl)trigger).CronExpressionString);
      host.StopAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public void inline_job_data_test()
    {

      var host = new HostBuilder()
       .ConfigureServices(s =>
       {
         s.AddQuartz().AddJob<MyJob>(o =>
         {
           o.Identity = "My-Id";
           o.CronSchedule = Cron.EveryHour();
           o.Data.Add("MyData", 123);
         });
       })
      .Build();

      host.Start();

      var scheduler = host.Services.GetRequiredService<ISchedulerFactory>().GetScheduler().GetAwaiter().GetResult();
      var job = scheduler.GetJobDetail(new JobKey("My-Id")).GetAwaiter().GetResult();
      Assert.True(job.JobDataMap.ContainsKey("MyData"));
      Assert.Equal(123, job.JobDataMap.GetInt("MyData"));
      host.StopAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public void config_job_must_exist()
    {

      var host = new HostBuilder()
        .ConfigureAppConfiguration(c =>
        {
          c.AddJsonFile("appsettings.json", optional: true);
        })
        .ConfigureServices(s =>
        {
          s.AddQuartz().AddJob<MyJob>();
        })
      .Build();

      host.Start();

      var scheduler = host.Services.GetRequiredService<ISchedulerFactory>().GetScheduler().GetAwaiter().GetResult();
      Assert.True(scheduler.CheckExists(new JobKey("My-Id")).GetAwaiter().GetResult());

      host.StopAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public void config_job_cronschedule_test()
    {

      var host = new HostBuilder()
        .ConfigureAppConfiguration(c =>
        {
          c.AddJsonFile("appsettings.json", optional: true);
        })
        .ConfigureServices(s =>
        {
          s.AddQuartz().AddJob<MyJob>();
        })
      .Build();

      host.Start();

      var scheduler = host.Services.GetRequiredService<ISchedulerFactory>().GetScheduler().GetAwaiter().GetResult();
      var trigger = scheduler.GetTriggersOfJob(new JobKey("My-Id")).GetAwaiter().GetResult().First();
      Assert.Equal(Cron.EverySomeSeconds(30), ((CronTriggerImpl)trigger).CronExpressionString);
      host.StopAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public void config_job_data_test()
    {
      var host = new HostBuilder()
        .ConfigureAppConfiguration(c =>
        {
          c.AddJsonFile("appsettings.json", optional: true);
        })
        .ConfigureServices(s =>
        {
          s.AddQuartz().AddJob<MyJob>();
        })
      .Build();

      host.Start();

      var scheduler = host.Services.GetRequiredService<ISchedulerFactory>().GetScheduler().GetAwaiter().GetResult();
      var job = scheduler.GetJobDetail(new JobKey("My-Id")).GetAwaiter().GetResult();
      Assert.True(job.JobDataMap.ContainsKey("MyData"));
      Assert.Equal(123, job.JobDataMap.GetInt("MyData"));
      host.StopAsync().GetAwaiter().GetResult();
    }

  }
}
