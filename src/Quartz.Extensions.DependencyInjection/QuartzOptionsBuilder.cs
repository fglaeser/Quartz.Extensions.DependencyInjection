using System.Collections.Specialized;

namespace Quartz.Extensions.DependencyInjection
{
  public class QuartzOptionsBuilder
  {
    public NameValueCollection Properties { get; set; }
    public bool WaitForJobsToComplete { get; set; }
    public bool EnsureDatabaseIsCreated { get; set; }
  }
}
