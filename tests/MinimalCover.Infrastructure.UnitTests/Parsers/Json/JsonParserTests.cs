using System.Collections.Generic;

using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers;
using MinimalCover.UnitTests.Utils;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Xunit;

namespace MinimalCover.Infrastructure.UnitTests.Parsers.Json
{
  /// <summary>
  /// This class tests the provided <see cref="JsonParser"/> implementation
  /// from <see cref="DependencyInjection"/>.
  /// 
  /// Currently, the test data rely on the schema defined in
  /// <see cref="Infrastructure.Parsers.Json.Converter.FdSetConverter.SchemaFilePath"/>
  /// </summary>
  public class JsonParserTests
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
    private readonly JsonParser m_jsonParser;

    /// <summary>
    /// Constructor
    /// </summary>
    public JsonParserTests()
    {
      // Create empty configuration since json parser
      // currently doesn't support configurations
      var config = new ConfigurationBuilder().Build();

      var services = new ServiceCollection();
      services.AddInfrastructure(config);
      var provider = services.BuildServiceProvider();

      var getParser = provider.GetService<GetParser>();
      m_jsonParser = (JsonParser)getParser(ParseFormat.Json);
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
    public void Parse_InvalidJsonString_ThrowsParserException(string value)
    {
      Assert.Throws<ParserException>(() => m_jsonParser.Parse(value));
    }
    
    [Theory]
    [MemberData(nameof(ValidJsonTheoryData))]
    public void Parse_ValidString_ReturnsExpectedFdSet(ParsedJsonFdsTestData testData)
    {
      var parsedFds = m_jsonParser.Parse(testData.Value);
      Assert.Equal(testData.ExpectedFds, parsedFds);
    }

  }
}
