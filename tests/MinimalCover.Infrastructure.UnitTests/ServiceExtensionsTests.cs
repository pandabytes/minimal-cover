using System;

using MinimalCover.Application.Algorithms;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Parsers.Settings;
using static MinimalCover.UnitTests.Utils.ConfigurationUtils;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace MinimalCover.Infrastructure.UnitTests
{
  public class ServiceExtensionsTests
  {
    [Fact]
    public void AddParsers_GetTextParser_TextParserIsRegistered()
    {
      var settings = new TextParserSettings
      {
        AttributeSeparator = ",",
        FdSeparator = ";",
        LeftRightSeparator = "-->"
      };

      var config = CreateConfig(settings, TextParserSettings.SectionPath);
      var services = new ServiceCollection()
                      .AddParsers(config);
      var provider = services.BuildServiceProvider();

      // No need to call any Xunit.Assert because GetRequiredService
      // would throw an exception if TextParser is not registered
      provider.GetRequiredService<TextParser>();
    }

    [Fact]
    public void AddParsers_GetJsonParser_JsonParserIsRegistered()
    {
      var settings = new JsonParserSettings
      {
        SchemaFilePath = @"Parsers\Json\fd-schema.json"
      };

      var config = CreateConfig(settings, JsonParserSettings.SectionPath);
      var services = new ServiceCollection()
                      .AddParsers(config);
      var provider = services.BuildServiceProvider();

      // No need to call any Xunit.Assert because GetRequiredService
      // would throw an exception if JsonParser is not registered
      provider.GetRequiredService<JsonParser>();
    }

    [Fact]
    public void AddMinimalCoverAlgs_GetMinimalCover_MinimalCoverIsRegistered()
    {
      var services = new ServiceCollection()
                      .AddMinimalCoverAlgs();
      var provider = services.BuildServiceProvider();

      // No need to call any Xunit.Assert because GetRequiredService
      // would throw an exception if IMinimalCover is not registered
      provider.GetRequiredService<IMinimalCover>();
    }

  }
}
