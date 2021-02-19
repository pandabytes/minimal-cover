using System;

using MinimalCover.Application.Algorithms;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Parsers.Settings;

using MinimalCover.Infrastructure.Algorithms;
using MinimalCover.Infrastructure.Parsers.Text;
using MinimalCover.Infrastructure.Parsers.Json.Converter;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace MinimalCover.UI.Console
{
  public class Startup
  {
    private readonly IConfiguration m_configuration;

    public Startup(IConfiguration configuration)
    {
      m_configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      // Register text parser
      services.AddTransient<TextParser>(provider => {
        var settings = m_configuration.GetSection(TextParserSettings.TextParser)
                                      .Get<TextParserSettings>();
        return new DefaultTextParser(settings);
      });

      // Register json parser
      services.AddTransient<FdSetConverter>();
      services.AddTransient<JsonParser>(provider => {
        var converter = provider.GetService<FdSetConverter>();
        return new JsonConverterParser(converter);
      });

      services.AddTransient<GetParser>(serviceProvider => format => {
        switch (format)
        {
          case ParseFormat.Text:
            return serviceProvider.GetService<TextParser>();
          case ParseFormat.Json:
            return serviceProvider.GetService<JsonParser>();
          default:
            throw new NotSupportedException($"Format \"{format}\" is not supported yet");
        }
      });

      services.AddTransient<IMinimalCover, DefaultMinimalCover>();
    }
  }
}
