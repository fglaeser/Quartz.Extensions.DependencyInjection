using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Quartz.Extensions.DependencyInjection;

namespace ConsoleApp.Test
{
  class Program
  {
    static void Main(string[] args)
    {
      var host = new HostBuilder()
      .ConfigureAppConfiguration(c =>
      {
        c.AddJsonFile("appsettings.json", optional: true);
      })
      .ConfigureServices(s =>
      {
        s.AddQuartz(o =>
        {
          o.WaitForJobsToComplete = true;
          o.UseSqlite("Data Source=db.3db");
        })
        //.AddJob<MyJob>()
        .AddJob<MyJobWithContext>()
        .AddJob<MyJobNoConfigFromFile>(o => {
          o.Identity = "MyJobInLine";
          o.CronSchedule = Cron.EverySomeSeconds(10);
        });
      })
      .Build();
      
      Console.WriteLine("Test Console Starting...");
      host.Run();
    }
  }
}
