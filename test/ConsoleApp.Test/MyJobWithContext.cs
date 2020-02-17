using Quartz;
using System;
using System.Threading.Tasks;

namespace ConsoleApp.Test
{
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