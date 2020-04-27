namespace Quartz.Extensions.DependencyInjection
{
  public static class Cron
  {
    /// <summary>
    /// Returns cron expression that fires every second.
    /// </summary>
    public static string EverySecond()
    {
      return "* * * ? * * *";
    }

    /// <summary>
    /// Returns cron expression that fires every &lt;<paramref name="seconds"></paramref>&gt; seconds.
    /// </summary>
    public static string EverySomeSeconds(int seconds)
    {
      return $"0/{seconds} * * ? * * *";
    }

    /// <summary>
    /// Returns cron expression that fires every minute.
    /// </summary>
    public static string EveryMinute()
    {
      return $"0 * * ? * * *";
    }

    /// <summary>
    /// Returns cron expression that fires every &lt;<paramref name="minutes"></paramref>&gt; minutes.
    /// </summary>
    public static string EverySomeMinutes(int minutes)
    {
      return $"0 */{minutes} * ? * * *";
    }

    /// <summary>
    /// Returns cron expression that fires every hour at the first minute.
    /// </summary>
    public static string EveryHour()
    {
      return "0 0 * ? * * *";
    }

    /// <summary>
    /// Returns cron expression that fires every &lt;<paramref name="hours"></paramref>&gt; houres.
    /// </summary>
    public static string EverySomeHours(int hours)
    {
      return $"0 0 */{hours} ? * * *";
    }

    /// <summary>
    ///  Returns cron expression that fires every day at 00:00.
    /// </summary>
    public static string EveryDay()
    {
      return "0 0 0 ? * * *";
    }
  }
}
