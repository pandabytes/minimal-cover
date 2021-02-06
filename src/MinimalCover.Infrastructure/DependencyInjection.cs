using System;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Algorithms;
using Microsoft.Extensions.DependencyInjection;

namespace MinimalCover.Infrastructure
{
  public static class DependencyInjection
  {
    public static void AddDependencies(IServiceCollection services)
    {
      services.AddTransient<IParser>(parser => new Parsers.TextParser());
      services.AddTransient<IMinimalCover, Algorithms.DefaultMinimalCover>();
    }
  }
}
