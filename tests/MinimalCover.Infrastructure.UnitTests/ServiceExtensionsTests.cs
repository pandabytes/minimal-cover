using System;

using MinimalCover.Application;
using MinimalCover.Application.Algorithms;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Parsers.Settings;
using static MinimalCover.Tests.Utils.ConfigurationUtils;

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

    [Theory]
    [InlineData(ParseFormat.Json, typeof(JsonParser))]
    [InlineData(ParseFormat.Text, typeof(TextParser))]
    public void AddParsers_GetAvailableParser_ReturnedParserMatchesParseFormat(ParseFormat parseFormat, Type expectedParserType)
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
      Assert.IsAssignableFrom(expectedParserType, getParser(parseFormat));
    }

    [Theory]
    [InlineData(ParseFormat.Yaml)]
    public void AddParsers_GetUnavailableParser_ThrowNotSupportedException(ParseFormat parseFormat)
    {
      var provider = new ServiceCollection()
                      .AddParsers(EmptyConfiguration)
                      .BuildServiceProvider();

      var getParser = provider.GetRequiredService<GetParser>();
      Assert.Throws<NotSupportedException>(() => getParser(parseFormat));
    }

    [Fact]
    public void AddMinimalCover_GetMinimalCover_MinimalCoverIsRegistered()
    {
      var services = new ServiceCollection()
                      .AddMinimalCover();
      var provider = services.BuildServiceProvider();

      // No need to call any Xunit.Assert because GetRequiredService
      // would throw an exception if IMinimalCover is not registered
      provider.GetRequiredService<IMinimalCover>();
    }

    [Fact]
    public void AddMinimalCover_GetMinimalCoverApp_MinimalCoverAppIsRegistered()
    {
      var services = new ServiceCollection()
                      .AddMinimalCover();
      var provider = services.BuildServiceProvider();

      // No need to call any Xunit.Assert because GetRequiredService
      // would throw an exception if IMinimalCover is not registered
      provider.GetRequiredService<MinimalCoverApp>();
    }

  }
}
