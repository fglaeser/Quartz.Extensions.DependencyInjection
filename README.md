# Quartz.Extensions.DependencyInjection
Helper to register Quartz Scheduler and Jobs into the .NET Core dependency injection container.

| NuGet Version  | Downloads | Build Status |
| ------------- | ------------- |-----------|
| [![NuGet Version and Downloads count](https://img.shields.io/nuget/vpre/Quartz.Extensions.DependencyInjection.svg)](http://www.nuget.org/packages/Quartz.Extensions.DependencyInjection/)|[![NuGet Download count](https://img.shields.io/nuget/dt/Quartz.Extensions.DependencyInjection.svg)](http://www.nuget.org/packages/Quartz.Extensions.DependencyInjection/)|[![Build Status](https://travis-ci.org/fglaeser/Quartz.Extensions.DependencyInjection.svg?branch=develop)](https://travis-ci.org/fglaeser/Quartz.Extensions.DependencyInjection)|

# Examples
### Add Quartz scheduler and Job to de IoC Container

```csharp
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
        s.AddJob<MyJob>(); //You can add all the jobs you need.
      })
      .Build();
      
      Console.WriteLine("Test Console Starting...");
      host.Run();
    }
```
You need to add some data in the appsettings.json file in order to configure the jobs.
```json
{
  "Quartz": {
    "Jobs": {
      "MyJob": {
        "Identity": "My-Id",
        "CronSchedule": "0/10 * * ? * * *",
        "StartNow": true
      }
    }
  }
}
```
Notice that "MyJob" is exactly the same name of the Job class previously registered in the IoC container.
