﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MinimalCover.Core.Parsers.Xunit
{
  public class JsonParserTests
  {
    public class ParsedFdsTestData
    {
      public string Value { get; set; }

      public ISet<FunctionalDependency> ExpectedFds;
    }

    public static TheoryData<ParsedFdsTestData> ParseTheoryData =
      new TheoryData<ParsedFdsTestData>()
      {
        new ParsedFdsTestData()
        {
          Value = "[{'left': ['A'], 'right': ['B']}]",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A", "B")
          }
        },
        new ParsedFdsTestData()
        {
          Value = "[{'left': ['A', 'E'], 'right': ['B', 'D']}]",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A,E", "B,D")
          }
        },
        new ParsedFdsTestData()
        {
          Value = "[{'left': ['A'], 'right': ['B']}," +
                   "{'left': ['C'], 'right': ['E']}," +
                   "{'left': ['C'], 'right': ['A']}," +
                   "{'left': ['B', 'D'], 'right': ['F']}," +
                   "{'left': ['E'], 'right': ['G', 'H']}]",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A", "B"),
            new FunctionalDependency("C", "E"),
            new FunctionalDependency("C", "A"),
            new FunctionalDependency("B,D", "F"),
            new FunctionalDependency("E", "G,H"),
          }
        }
      };

    [Theory]
    [InlineData("[]")]
    [InlineData("[{}]")]
    [InlineData("[{'left': []}]")]
    [InlineData("[{'right': []}]")]
    [InlineData("[{'left': [], 'right': []}]")]
    public void Parse_InvalidJsonString_ThrowsArgumentException(string value)
    {
      Assert.Throws<ArgumentException>(() => JsonParser.Parse(value));
    }

    [Theory]
    [MemberData(nameof(ParseTheoryData))]
    public void Parse_ValidString_ReturnsFdSet(ParsedFdsTestData testData)
    {
      var fdsSet = JsonParser.Parse(testData.Value);
      Assert.Equal(testData.ExpectedFds, fdsSet);
    }

  }
}
