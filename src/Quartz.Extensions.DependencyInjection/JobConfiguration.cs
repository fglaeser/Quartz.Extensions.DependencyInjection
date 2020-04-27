using System;

namespace Quartz.Extensions.DependencyInjection
{
  public class JobConfiguration
  {
    public Type JobType { get; set; }
    public JobOptions Options { get; set; }
  }
}
