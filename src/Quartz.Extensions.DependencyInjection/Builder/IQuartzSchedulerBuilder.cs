using System;

namespace Quartz.Extensions.DependencyInjection.Builder
{
  public interface IQuartzSchedulerBuilder 
  {
    IQuartzSchedulerBuilder AddJob<TJob>(Action<JobOptions> action = null) where TJob : IJob;
  }
}
