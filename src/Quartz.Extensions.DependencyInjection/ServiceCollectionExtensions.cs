using Quartz;
using Quartz.Extensions.DependencyInjection;
using Quartz.Extensions.DependencyInjection.Builder;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Specialized;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class ServiceCollectionExtensions
  {
    public static IQuartzSchedulerBuilder AddQuartz(this IServiceCollection services, Action<QuartzOptionsBuilder> options = null)
    {
      var builder = new QuartzOptionsBuilder
      {
        Properties = new NameValueCollection {  }
      };

      options?.Invoke(builder);

      services.AddSingleton<ISchedulerFactory>(new StdSchedulerFactory(builder.Properties));
      services.AddSingleton<IJobFactory, ScopedJobFactory>();
      services.AddHostedService<SchedulerHostedService>();
      services.Configure<QuartzOptions>(o =>
      {
        o.WaitForJobsToComplete = builder.WaitForJobsToComplete;
        o.EnsureDatabaseIsCreated = builder.EnsureDatabaseIsCreated;
      });

      return new QuartzSchedulerBuilder(services);
    }

    public static QuartzOptionsBuilder UseSqlite(this QuartzOptionsBuilder builder, 
      string connectString, string tablePrefix = "QRTZ_", bool ensureDatabaseIsCreated = true)
    {
      builder.Properties.Set("quartz.jobStore.useProperties", "true");
      builder.Properties.Set("quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz");
      builder.Properties.Set("quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz");
      builder.Properties.Set("quartz.jobStore.dataSource", "myDs");
      builder.Properties.Set("quartz.dataSource.myDs.provider", "SQLite-Microsoft");
      builder.Properties.Set("quartz.jobStore.tablePrefix", tablePrefix);
      builder.Properties.Set("quartz.serializer.type", "json");
      builder.Properties.Set("quartz.dataSource.myDs.connectionString", connectString);

      builder.EnsureDatabaseIsCreated = ensureDatabaseIsCreated;
      return builder;
    }
  }
}
