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
    private readonly ServiceProvider m_provider;

    public ServiceExtensionsTests()
    {
      var parserSettings = new ParserSettings
      {
        TextParser = new TextParserSettings
        {
          AttributeSeparator = ",",
          FdSeparator = ";",
          LeftRightSeparator = "-->"
        },
        JsonParser = new JsonParserSettings
        {
          SchemaFilePath = "Parsers/Json/fd-schema.json"
        }
      };

      var config = CreateConfig(parserSettings, ParserSettings.SectionPath);
      m_provider = new ServiceCollection()
                      .AddParsers(config)
                      .AddMinimalCover()
                      .BuildServiceProvider();
    }

    [Fact]
    public void AddParsers_GetTextParser_TextParserIsRegistered()
    {
      // No need to call any Xunit.Assert because GetRequiredService
      // would throw an exception if TextParser is not registered
      m_provider.GetRequiredService<TextParser>();
    }

    [Fact]
    public void AddParsers_GetJsonParser_JsonParserIsRegistered()
    {
      // No need to call any Xunit.Assert because GetRequiredService
      // would throw an exception if JsonParser is not registered
      m_provider.GetRequiredService<JsonParser>();
    }

    [Fact]
    public void AddParsers_GetParser_GetParserDelegateIsRegistered()
    {
      var provider = new ServiceCollection()
                      .AddParsers(EmptyConfiguration)
                      .BuildServiceProvider();

      // No need to call any Xunit.Assert because GetRequiredService
      // would throw an exception if GetParser is not registered
      provider.GetRequiredService<GetParser>();
    }

    [Theory]
    [InlineData(typeof(JsonParser))]
    [InlineData(typeof(TextParser))]
    public void AddParsers_IndividualParserSettingsNotRegistered_ThrowsInvalidOperationException(Type parserType)
    {
      var parserSettings = new ParserSettings();

      var config = CreateConfig(parserSettings, ParserSettings.SectionPath);
      var provider = new ServiceCollection()
                      .AddParsers(config)
                      .BuildServiceProvider();
      Assert.Throws<InvalidOperationException>(() => provider.GetService(parserType));
    }

    [Theory]
    [InlineData(ParseFormat.Json, typeof(JsonParser))]
    [InlineData(ParseFormat.Text, typeof(TextParser))]
    public void AddParsers_GetAvailableParser_ReturnedParserMatchesParseFormat(ParseFormat parseFormat, Type expectedParserType)
    {
      var getParser = m_provider.GetRequiredService<GetParser>();
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
      // No need to call any Xunit.Assert because GetRequiredService
      // would throw an exception if IMinimalCover is not registered
      m_provider.GetRequiredService<IMinimalCover>();
    }

    [Fact]
    public void AddMinimalCover_GetMinimalCoverApp_MinimalCoverAppIsRegistered()
    {
      // No need to call any Xunit.Assert because GetRequiredService
      // would throw an exception if IMinimalCover is not registered
      m_provider.GetRequiredService<MinimalCoverApp>();
    }

  }
}
