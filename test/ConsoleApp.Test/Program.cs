using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz.Extensions.DependencyInjection;
using System;

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
        s.AddQuartz();
        s.AddJob<MyJob>();
      })
      .Build();
      
      Console.WriteLine("Test Console Starting...");
      host.Run();
    }
  }
}
