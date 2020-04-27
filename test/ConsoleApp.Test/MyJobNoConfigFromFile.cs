using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test
{
  public class MyJobNoConfigFromFile : IJob
  {
    public Task Execute(IJobExecutionContext context)
    {
      Console.WriteLine("Job Inline");

      JobDataMap dataMap = context.JobDetail.JobDataMap;

      var testId = dataMap.GetInt("ID");
      Console.WriteLine($"Context: ID {testId}");

      return Task.CompletedTask;
    }
  }
}
