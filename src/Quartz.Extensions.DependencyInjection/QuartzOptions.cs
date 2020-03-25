using System;
using System.Collections.Generic;

namespace Quartz.Extensions.DependencyInjection
{
  public class QuartzOptions
  {
    public ICollection<Type> Jobs { get; } = new List<Type>();
  }
}
