using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using System;
using System.Collections.Concurrent;

namespace Quartz.Extensions.DependencyInjection
{
  public class ScopedJobFactory : IJobFactory
  {
    private readonly IServiceProvider _serviceProvider;

    private readonly ConcurrentDictionary<IJob, IServiceScope> _serviceScopes = new ConcurrentDictionary<IJob, IServiceScope>();

    public ScopedJobFactory(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
      var jobType = bundle.JobDetail.JobType;
      var scope = _serviceProvider.CreateScope();

      var job = (IJob)(scope.ServiceProvider.GetService(jobType)
                       ?? ActivatorUtilities.CreateInstance(scope.ServiceProvider, jobType));

      _serviceScopes.TryAdd(job, scope);
      return job;
    }

    public void ReturnJob(IJob job)
    {
      if (job is IDisposable disposable)
      {
        disposable.Dispose();
      }

      if (_serviceScopes.TryRemove(job, out var scope))
      {
        scope.Dispose();
      }
    }
  }
}
