using System;
using System.Collections.Generic;
using Xunit;

using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers;
using MinimalCover.Infrastructure.Parsers;
using MinimalCover.UnitTests.Utils;

namespace MinimalCover.Infrastructure.UnitTests.Parsers
{
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

    public static TheoryData<ParsedJsonFdsTestData> ParsedJsonTheoryData =
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

    [Fact]
    public void Format_SimpleGet_ReturnsJsonFormat()
    {
      IParser jsonParser = new JsonParser();
      Assert.Equal(ParseFormat.Json, jsonParser.Format);
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
    public void Parse_InvalidJsonString_ThrowsArgumentException(string value)
    {
      IParser jsonParser = new JsonParser();
      Assert.Throws<ParserException>(() => jsonParser.Parse(value));
    }
    
    [Theory]
    [MemberData(nameof(ParsedJsonTheoryData))]
    public void Parse_ValidString_ReturnsExpectedFdSet(ParsedJsonFdsTestData testData)
    {
      IParser jsonParser = new JsonParser();
      var parsedFds = jsonParser.Parse(testData.Value);
      Assert.Equal(testData.ExpectedFds, parsedFds);
    }

  }
}
