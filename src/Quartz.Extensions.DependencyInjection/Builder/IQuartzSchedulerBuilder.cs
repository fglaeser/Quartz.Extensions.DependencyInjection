namespace Quartz.Extensions.DependencyInjection.Builder
{
  public interface IQuartzSchedulerBuilder 
  {
    IQuartzSchedulerBuilder AddJob<TJob>() where TJob : IJob;
  }
}
