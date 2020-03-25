using Microsoft.Extensions.DependencyInjection;

namespace Quartz.Extensions.DependencyInjection.Builder
{
  public class QuartzSchedulerBuilder : IQuartzSchedulerBuilder
  {
    public QuartzSchedulerBuilder(IServiceCollection services)
      => Services = services;
    public IServiceCollection Services { get; }

    public IQuartzSchedulerBuilder AddJob<TJob>() where TJob : IJob
    {
      Services.Configure<QuartzOptions>(options =>
      {
        options.Jobs.Add(typeof(TJob));
      });
      return this;
    }
  }
}
