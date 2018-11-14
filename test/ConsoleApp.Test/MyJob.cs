using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test
{
  public class MyJob : IJob
  {
    public Task Execute(IJobExecutionContext context)
    {
      Console.WriteLine("Job Execute");
      return Task.CompletedTask;
    }
  }
}
