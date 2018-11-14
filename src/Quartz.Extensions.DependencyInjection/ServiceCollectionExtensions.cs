using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz.Spi;
using System.Linq;

namespace Quartz.Extensions.DependencyInjection
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddQuartz(this IServiceCollection services)
    {
      services.AddSingleton<IJobFactory, QuartzJobFactory>();
      services.AddTransient<IHostedService>(s =>
      {
        var jobs = s.GetServices<IJob>().Select(j => j.GetType()).ToArray();
        return new SchedulerHostedService(s.GetService<IJobFactory>(), s.GetService<IConfiguration>(), jobs);
      });
      return services;
    }

    public static IServiceCollection AddJob<TJob>(this IServiceCollection services) where TJob : IJob
    {
      services.Add(new ServiceDescriptor(typeof(TJob), typeof(TJob), ServiceLifetime.Singleton));
      services.AddSingleton(s => (IJob)s.GetService(typeof(TJob)));
      return services;
    }
  }
}
