using System.Collections.Generic;

using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers;
using MinimalCover.UnitTests.Utils;
using MinimalCover.Application.Parsers.Settings;
using static MinimalCover.Infrastructure.UnitTests.ConfigurationUtils;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace MinimalCover.Infrastructure.UnitTests.Parsers.Json
{
  /// <summary>
  /// This class encapsulates all the tests for any class
  /// that inhereit <see cref="JsonParser"/>. 
  /// 
  /// To test an implementation of <see cref="JsonParser"/>, simply
  /// inhereit <see cref="JsonParserTests"/> and provide
  /// the implementation to the tests
  /// </summary>
  public abstract class JsonParserTests
  {
    /// <summary>
    /// This class is only used to stored data for testing purposes.
    /// </summary>
    public class ParsedJsonFdsTestData
    {
      public string Value { get; set; }

      public ISet<FunctionalDependency> ExpectedFds { get; set; }
    }

    /// <summary>
    /// Provide valid JSON test data
    /// </summary>
    public static TheoryData<ParsedJsonFdsTestData> ValidJsonTheoryData =
      new TheoryData<ParsedJsonFdsTestData>()
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

    [Fact]
    public abstract void Constructor_InvalidArguments_ThrowsArgumentException();

    [Fact]
    public abstract void Format_SimpleGet_ReturnsJsonFormat();

    [Theory]
    [InlineData("[]")]
    [InlineData("[{}]")]
    [InlineData("[{'left': []}]")]
    [InlineData("[{'right': []}]")]
    [InlineData("[{'left': [], 'right': []}]")]
    [InlineData("[{'left': ['A'], 'right': ['B']}, {'left': ['A'], 'right': []}]")]
    [InlineData("[{'left': ['A'], 'right': ['B']}, {'left': [], 'right': ['B']}]")]
    [InlineData("[{'left': ['A'], 'right': ['B']}, {'left': [], 'right': []}]")]
    public abstract void Parse_InvalidJsonString_ThrowsParserException(string value);

    [Theory]
    [MemberData(nameof(ValidJsonTheoryData))]
    public abstract void Parse_ValidString_ReturnsExpectedFdSet(ParsedJsonFdsTestData testData);

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
