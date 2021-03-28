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
      var provider = new ServiceCollection()
                      .AddParsers(config)
                      .BuildServiceProvider();

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
      var provider = new ServiceCollection()
                      .AddParsers(config)
                      .BuildServiceProvider();

      // No need to call any Xunit.Assert because GetRequiredService
      // would throw an exception if JsonParser is not registered
      provider.GetRequiredService<JsonParser>();
    }

    [Fact]
    public void AddParsers_GetParser_GetParserIsRegistered()
    {
      var provider = new ServiceCollection()
                      .AddParsers(EmptyConfiguration)
                      .BuildServiceProvider();

      // No need to call any Xunit.Assert because GetRequiredService
      // would throw an exception if GetParser is not registered
      provider.GetRequiredService<GetParser>();
    }

    [Fact]
    public void AddParsers_GetAvailableParser_ReturnedParserMatchesParseFormat()
    {
      var textParserSettings = new TextParserSettings
      {
        AttributeSeparator = ",",
        FdSeparator = ";",
        LeftRightSeparator = "-->"
      };

      var jsonParserSettings = new JsonParserSettings
      {
        SchemaFilePath = @"Parsers\Json\fd-schema.json"
      };

      var config = CreateConfig(textParserSettings, TextParserSettings.SectionPath)
                    .UpdateConfig(jsonParserSettings, JsonParserSettings.SectionPath);

      var provider = new ServiceCollection()
                      .AddParsers(config)
                      .BuildServiceProvider();

      var getParser = provider.GetRequiredService<GetParser>();

      Assert.IsAssignableFrom<TextParser>(getParser(ParseFormat.Text));
      Assert.IsAssignableFrom<JsonParser>(getParser(ParseFormat.Json));
    }

    [Fact]
    public void AddParsers_GetUnavailableParser_ThrowNotSupportedException()
    {
      var provider = new ServiceCollection()
                      .AddParsers(EmptyConfiguration)
                      .BuildServiceProvider();

      var getParser = provider.GetRequiredService<GetParser>();

      Assert.Throws<NotSupportedException>(() => getParser(ParseFormat.Yaml));
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
