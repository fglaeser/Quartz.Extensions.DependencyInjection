# Quartz.Extensions.DependencyInjection
Helper to register Quartz Scheduler and Jobs into the .NET Core dependency injection container.

| NuGet Version  | Downloads | Build Status |
| ------------- | ------------- |-----------|
| [![NuGet Version and Downloads count](https://img.shields.io/nuget/vpre/Quartz.Extensions.DependencyInjection.svg)](http://www.nuget.org/packages/Quartz.Extensions.DependencyInjection/)|[![NuGet Download count](https://img.shields.io/nuget/dt/Quartz.Extensions.DependencyInjection.svg)](http://www.nuget.org/packages/Quartz.Extensions.DependencyInjection/)|[![Build Status](https://travis-ci.com/fglaeser/Quartz.Extensions.DependencyInjection.svg?branch=develop)](https://travis-ci.com/fglaeser/Quartz.Extensions.DependencyInjection)|

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
        s.AddQuartz()
         .AddJob<MyJob>(); //You can add all the jobs you need in a fluent way
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

## Passing custom data to Job
Thanks to @rektifier you can now pass custom data to the job. Just add the custom data in the appsettings file like this:
 
```json
{
  "Quartz": {
    "Jobs": {
      "MyJobWithContext": {
        "Identity": "My-Context-Id",
        "CronSchedule": "0/10 * * ? * * *",
        "StartNow": true,
        "Data": {
          "TestId": 123,
          "TestName": "John"
        }
      }
    }
  }
}

```

Then access to the data through the JobDataMap.

```c#

    public class MyJobWithContext : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("MyJobWithContext Execute");

            JobDataMap dataMap = context.JobDetail.JobDataMap;

            var testId = dataMap.GetInt("TestId");
            var testName = dataMap.GetString("TestName");

            Console.WriteLine($"Context: TestId:{testId}, TestName:{testName}");
            return Task.CompletedTask;
        }
    }
}

```
