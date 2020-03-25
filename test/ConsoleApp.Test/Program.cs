using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
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
        s.AddQuartz()
        .AddJob<MyJob>()
        .AddJob<MyJobWithContext>();
      })
      .Build();
      
      Console.WriteLine("Test Console Starting...");
      host.Run();
    }
  }
}
