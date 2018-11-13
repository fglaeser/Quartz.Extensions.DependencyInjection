using Microsoft.Extensions.DependencyInjection;
using System;

namespace Quartz.Extensions.DependencyInjection
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddQuartz(this IServiceCollection services)
    {
      return services;
    }
  }
}
