﻿using System.Collections.Generic;

namespace Quartz.Extensions.DependencyInjection
{
  public class QuartzOptions
  {
    public ICollection<JobConfiguration> Jobs { get; } = new List<JobConfiguration>();
    public bool WaitForJobsToComplete { get; set; }
    public bool EnsureDatabaseIsCreated { get; set; }
  }
}
