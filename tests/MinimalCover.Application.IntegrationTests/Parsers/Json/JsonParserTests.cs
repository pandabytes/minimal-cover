using System;
using System.Reflection;
using System.Collections.Generic;

using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Parsers.Settings;
using MinimalCover.Tests.Utils;
using static MinimalCover.Tests.Utils.ConfigurationUtils;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Moq;
using Xunit;

namespace MinimalCover.Application.IntegrationTests.Parsers.Json
{
  public class JsonParserTests
  {
    /// <summary>
    /// This class is only used to stored data for testing purposes.
    /// </summary>
    public class ParsedJsonFdsTestData
    {
      public string Value { get; set; } = null!;

      public ISet<FunctionalDependency> ExpectedFds { get; set; } = null!;
    }

    /// <summary>
    /// Provide valid JSON test data
    /// </summary>
    public static readonly TheoryData<ParsedJsonFdsTestData> ValidJsonTheoryData =
      new()
      {
        new ParsedJsonFdsTestData()
        {
          Value = "[{'left': ['A'], 'right': ['B']}]",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A", "B")
          }
        },

        new ParsedJsonFdsTestData()
        {
          Value = "[{'left': ['A', 'E'], 'right': ['B', 'D']}]",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A,E", "B,D")
          }
        },

        new ParsedJsonFdsTestData()
        {
          Value = "[{'left': ['A'], 'right': ['B']}," +
                   "{'left': ['C'], 'right': ['E']}," +
                   "{'left': ['C'], 'right': ['A']}," +
                   "{'left': ['B', 'D'], 'right': ['F']}," +
                   "{'left': ['E'], 'right': ['G', 'H']}]",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A", "B"),
            FuncDepUtils.ConstructFdFromString("C", "E"),
            FuncDepUtils.ConstructFdFromString("C", "A"),
            FuncDepUtils.ConstructFdFromString("B,D", "F"),
            FuncDepUtils.ConstructFdFromString("E", "G,H"),
          }
        }
      };

    /// <summary>
    /// Object under test
    /// </summary>
    protected JsonParser m_jsonParser;

    public JsonParserTests()
    {
      m_jsonParser = GetJsonParser(@"Parsers\Json\fd-schema.json");
    }

    [Fact]
    public void Constructor_NullSchemaPath_ThrowsArgumentException()
    {
      // Mock the abstract class so that we can use its constructor
      // The outer exception is thrown by Mock and the actual
      // exception of our code is the inner exception
      var settings = new JsonParserSettings();
      var ex = Assert.Throws<TargetInvocationException>(() => new Mock<JsonParser>(settings).Object);
      var actualEx = ex.InnerException;

      Assert.IsType<ArgumentException>(actualEx);
    }

    [Fact]
    public void Format_SimpleGet_ReturnsJsonFormat()
    {
      Assert.Equal(ParseFormat.Json, ((IParser)m_jsonParser).Format);
    }

    [Theory]
    [InlineData("[]")]
    [InlineData("[{}]")]
    [InlineData("[{'left': []}]")]
    [InlineData("[{'right': []}]")]
    [InlineData("[{'left': [], 'right': []}]")]
    [InlineData("[{'left': ['A'], 'right': ['B']}, {'left': ['A'], 'right': []}]")]
    [InlineData("[{'left': ['A'], 'right': ['B']}, {'left': [], 'right': ['B']}]")]
    [InlineData("[{'left': ['A'], 'right': ['B']}, {'left': [], 'right': []}]")]
    public void Parse_JsonStringNotMatchSchema_ThrowsParserException(string value)
    {
      Assert.Throws<ParserException>(() => m_jsonParser.Parse(value));
    }

    [Theory]
    [InlineData("[")]
    [InlineData("{")]
    [InlineData("[{'left': [, 'right': []}]")]
    [InlineData("[{'left': ['A'], 'right': ['B']}, 'left': ['A'], 'right': []}]")]
    [InlineData("[{'left': ['A'], 'right': ['B'}, {'left': [], 'right': ['B']}]")]
    [InlineData("[{'left': ['A'], 'right': ['B']}, {'left': [], 'right': ]}]")]
    public void Parse_BadJsonSyntax_ThrowsParserException(string value)
    {
      var ex = Assert.Throws<ParserException>(() => m_jsonParser.Parse(value));
      Assert.IsType<JsonReaderException>(ex.InnerException);
    }

    [Theory]
    [MemberData(nameof(ValidJsonTheoryData))]
    public void Parse_ValidString_ReturnsExpectedFdSet(ParsedJsonFdsTestData testData)
    {
      var parsedFds = m_jsonParser.Parse(testData.Value);
      Assert.Equal(testData.ExpectedFds, parsedFds);
    }

    /// <summary>
    /// Get the JSON parser via dependency injection
    /// </summary>
    /// <param name="schemaFilePath">Path to the schema file</param>
    /// <returns>The JSON parser</returns>
    protected static JsonParser GetJsonParser(string schemaFilePath)
    {
      var settings = new JsonParserSettings { SchemaFilePath = schemaFilePath };
      var config = CreateConfig(settings, JsonParserSettings.SectionPath);
      var dp = new DependencyInjection(config);
      return dp.Provider.GetRequiredService<JsonParser>();
    }
  }
}
