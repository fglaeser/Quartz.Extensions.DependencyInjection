# DEPRECATED

This is no longer supported, please consider using [Quartz.Net](https://github.com/quartznet/quartznet) instead.

# Quartz.Extensions.DependencyInjection
Helper to register Quartz Scheduler and Jobs into the .NET Core dependency injection container.

| NuGet Version  | Downloads | Build Status |
| ------------- | ------------- |-----------|
| [![No Maintenance Intended](http://unmaintained.tech/badge.svg)](http://unmaintained.tech/)|[![NuGet Download count](https://img.shields.io/nuget/dt/Quartz.Extensions.DependencyInjection.svg)](http://www.nuget.org/packages/Quartz.Extensions.DependencyInjection/)|[![Build Status](https://travis-ci.com/fglaeser/Quartz.Extensions.DependencyInjection.svg?branch=develop)](https://travis-ci.com/fglaeser/Quartz.Extensions.DependencyInjection)|

# Examples
## Add Quartz scheduler and Job to de IoC Container

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

## Inline Job registration

You can also add a Job to the container without the appsetting.json configuration.

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
         .AddJob<MyJob>(o => {
           o.Identity = "My-Id";
           o.CronSchedule = Cron.EverySomeSeconds(10);
           o.StartNow = true;
         });
      })
      .Build();
      
      Console.WriteLine("Test Console Starting...");
      host.Run();
    }
```

Notice that you can use de `Cron` helper class that provide common values for the `CronSchedule` property.

```csharp

Cron.EverySecond(); 
Cron.EverySomeSeconds(10); // Runs every 10 sencods
Cron.EveryMinute();
Cron.EverySomeMinutes(2); 
Cron.EveryHour();
Cron.EverySomeHours(3);
Cron.EveryDay(); // Runs every day at 00:00.

```
## Sqlite Storage

To use sqlite as storage you need to add the package `Microsoft.Data.Sqlite` to the project, then call the `UseSqlite` method like the example below:

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
        s.AddQuartz(o => {

          o.UseSqlite(connectionString: "Data Source=db.3db");

        })
         .AddJob<MyJob>(o => {
           o.Identity = "My-Id";
           o.CronSchedule = Cron.EverySomeSeconds(10);
           o.StartNow = true;
         });
      })
      .Build();
      
      Console.WriteLine("Test Console Starting...");
      host.Run();
    }
```

As default before the schedule starts the lib will ensure that the tables are created (the [`tables_sqlite.sql`](https://github.com/quartznet/quartznet/blob/master/database/tables/tables_sqlite.sql) file from the Quartz.Net repo is embedded). You can control this behavior with the `ensureDatabaseIsCreated` parameter in the `UseSqlite` method:

```csharp

  s.AddQuartz(o => {

    o.UseSqlite(connectionString: "Data Source=db.3db", ensureDatabaseIsCreated: false);

  })

```

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

## Inline Job Custom data

Custom data can also be passed using the inline job configuration:

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
         .AddJob<MyJobWithContext>(o => {
           o.Identity = "My-Context-Id";
           o.CronSchedule = Cron.EverySomeSeconds(10);
           o.StartNow = true;
           o.Data.Add("TestId", 123);
           o.Data.Add("TestName", "John");
         });
      })
      .Build();
      
      Console.WriteLine("Test Console Starting...");
      host.Run();
    }
```
