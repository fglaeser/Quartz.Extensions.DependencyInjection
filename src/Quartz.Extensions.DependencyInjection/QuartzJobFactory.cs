using Quartz.Spi;
using System;

namespace Quartz.Extensions.DependencyInjection
{
  public class QuartzJobFactory : IJobFactory
  {
    private readonly IServiceProvider _serviceProvider;

    public QuartzJobFactory(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
      var jobDetail = bundle.JobDetail;
      var job = (IJob)_serviceProvider.GetService(jobDetail.JobType);
      return job;
    }

    public void ReturnJob(IJob job)
    {
      (job as IDisposable)?.Dispose();
    }
  }
}
