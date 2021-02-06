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
      // Register parsers
      services.AddTransient<Parsers.TextParser>();
      services.AddTransient<GetParser>(serviceProvider => format => {
        switch (format)
        {
          case ParseFormat.Text:
            return serviceProvider.GetService<Parsers.TextParser>();
          default:
            throw new NotSupportedException($"Format \"{format}\" is not supported yet");
        }
      });

      // Register minimal cover algorithms
      services.AddTransient<IMinimalCover, Algorithms.DefaultMinimalCover>();
    }
  }
}
