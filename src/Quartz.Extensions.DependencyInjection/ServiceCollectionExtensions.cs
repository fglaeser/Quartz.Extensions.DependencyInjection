using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Extensions.DependencyInjection;
using Quartz.Extensions.DependencyInjection.Builder;
using Quartz.Spi;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class ServiceCollectionExtensions
  {

    public static IQuartzSchedulerBuilder AddQuartz(this IServiceCollection services)
    {
      services.AddSingleton<IJobFactory, ScopedJobFactory>();
      services.AddHostedService<SchedulerHostedService>();
      return new QuartzSchedulerBuilder(services);
    }
  }
}
