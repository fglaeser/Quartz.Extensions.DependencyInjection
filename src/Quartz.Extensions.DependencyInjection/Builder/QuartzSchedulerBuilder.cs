using Microsoft.Extensions.DependencyInjection;
using System;

namespace Quartz.Extensions.DependencyInjection.Builder
{
  public class QuartzSchedulerBuilder : IQuartzSchedulerBuilder
  {
    public QuartzSchedulerBuilder(IServiceCollection services)
      => Services = services;
    public IServiceCollection Services { get; }

    public IQuartzSchedulerBuilder AddJob<TJob>(Action<JobOptions> action = null) where TJob : IJob
    {
      Services.Configure<QuartzOptions>(options => {
        var jobConfiguration = new JobConfiguration
        {
          JobType = typeof(TJob)
        };
        if(action != null)
        {
          var jobOptions = new JobOptions();
          action?.Invoke(jobOptions);
          jobConfiguration.Options = jobOptions;
        }
        options.Jobs.Add(jobConfiguration);
      });
      return this;
    }
  }
}
