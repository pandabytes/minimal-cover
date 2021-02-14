using System;

using MinimalCover.Application.Parsers;
using MinimalCover.Application.Algorithms;
using MinimalCover.Infrastructure.Parsers.Text;
using MinimalCover.Infrastructure.Parsers.Json.Converter;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace MinimalCover.Infrastructure
{
  public static class DependencyInjection
  { 
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
      // Register text parser
      services.AddTransient(provider => {
        var textParserConfig = config.GetSection("textParser");
        var attrbSep = textParserConfig.GetValue<string>("attrbSep");
        var fdSep = textParserConfig.GetValue<string>("fdSep");
        var leftRightSep = textParserConfig.GetValue<string>("leftRightSep");
        return new DefaultTextParser(attrbSep, fdSep, leftRightSep);
      });

      // Register json parser
      services.AddTransient<FdSetConverter>();
      services.AddTransient(provider => {
        var converter = provider.GetService<FdSetConverter>();
        return new JsonConverterParser(converter);
      });
      
      services.AddTransient<GetParser>(serviceProvider => format => {
        switch (format)
        {
          case ParseFormat.Text:
            return serviceProvider.GetService<DefaultTextParser>();
          case ParseFormat.Json:
            return serviceProvider.GetService<JsonConverterParser>();
          default:
            throw new NotSupportedException($"Format \"{format}\" is not supported yet");
        }
      });

      // Register minimal cover algorithms
      services.AddTransient<IMinimalCover, Algorithms.DefaultMinimalCover>();
    }
  }
}
