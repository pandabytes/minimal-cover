using System;

using MinimalCover.Domain.Models;

using MinimalCover.Application.Algorithms;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Parsers.Settings;

using MinimalCover.Infrastructure.Algorithms;
using MinimalCover.Infrastructure.Parsers.Text;
using MinimalCover.Infrastructure.Parsers.Json.Converter;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;

namespace MinimalCover.Infrastructure
{
  /// <summary>
  /// Provide extensions to the class <see cref="IServiceCollection"/>.
  /// Mainly provide methods to register dependencies to parse <see cref="FunctionalDependency"/>
  /// and to find minimal cover
  /// </summary>
  public static class ServinceExtensions
  {
    /// <summary>
    /// Add parsers that can parse string to a collection of <see cref="FunctionalDependency"/>
    /// </summary>
    /// <param name="services">Services object</param>
    /// <param name="configuration">Configuration that contains settings for each parser</param>
    /// <returns>The passed in services object</returns>
    public static IServiceCollection AddParsers(this IServiceCollection services, IConfiguration configuration)
    {
      // Register text parser
      //services.Configure<TextParserSettings>(
      //  configuration.GetSection(TextParserSettings.TextParser));
      //services.AddSingleton(provider =>
      //  provider.GetRequiredService<IOptions<TextParserSettings>>().Value);
      services.AddTransient(p =>
      {
        var settings = new TextParserSettings();
        configuration.GetSection(TextParserSettings.SectionPath).Bind(settings);
        return settings;
      });

      services.AddTransient<TextParser, DefaultTextParser>();

      // Register json parser
      services.AddTransient(p =>
      {
        var settings = new JsonParserSettings();
        configuration.GetSection(JsonParserSettings.SectionPath).Bind(settings);
        return settings;
      });
      //services.Configure<JsonParserSettings>(
      //  configuration.GetSection(JsonParserSettings.JsonParser));
      //services.AddTransient(provider =>
      //  provider.GetRequiredService<IOptions<JsonParserSettings>>().Value);

      services.AddTransient<FdSetConverter>();
      services.AddTransient<JsonParser, JsonConverterParser>();

      return services;
    }

    /// <summary>
    /// Add minimal cover algorithms
    /// </summary>
    /// <param name="services">Services object</param>
    /// <returns>The passed in services object</returns>
    public static IServiceCollection AddMinimalCoverAlgs(this IServiceCollection services)
    {
      services.AddTransient<IMinimalCover, DefaultMinimalCover>();
      return services;
    }
  }
}
