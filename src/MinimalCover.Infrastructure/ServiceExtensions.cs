using System;
using System.Linq;

using MinimalCover.Domain.Models;

using MinimalCover.Application.Algorithms;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Parsers.Settings;

using MinimalCover.Infrastructure.Algorithms;
using MinimalCover.Infrastructure.Parsers.Text;
using MinimalCover.Infrastructure.Parsers.Json.Converter;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MinimalCover.Infrastructure
{
  /// <summary>
  /// Provide extensions to the class <see cref="IServiceCollection"/>.
  /// Mainly provide methods to register dependencies to parse <see cref="FunctionalDependency"/>
  /// and to find minimal cover
  /// </summary>
  public static class ServiceExtensions
  {
    /// <summary>
    /// Add parsers that can parse string to a collection of <see cref="FunctionalDependency"/>
    /// </summary>
    /// <param name="services">Services object</param>
    /// <param name="configuration">Configuration that contains settings for each parser</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configuration"/> is null</exception>
    /// <returns>The passed in services object</returns>
    public static IServiceCollection AddParsers(this IServiceCollection services, IConfiguration configuration)
    {
      _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

      // Register text parser
      services.Configure<TextParserSettings>(
        configuration.GetSection(TextParserSettings.SectionPath));
      services.AddSingleton(provider =>
        provider.GetRequiredService<IOptions<TextParserSettings>>().Value);

      services.AddTransient<TextParser, DefaultTextParser>();

      // Register json parser
      services.Configure<JsonParserSettings>(
        configuration.GetSection(JsonParserSettings.SectionPath));
      services.AddSingleton(provider =>
        provider.GetRequiredService<IOptions<JsonParserSettings>>().Value);

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
