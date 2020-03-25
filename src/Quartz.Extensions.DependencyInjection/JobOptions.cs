
namespace Quartz.Extensions.DependencyInjection
{
  public class JobOptions
  {
    public string Identity { get; set; }   
    public string CronSchedule { get; set; }
    public bool StartNow { get; set; }
  }
}
