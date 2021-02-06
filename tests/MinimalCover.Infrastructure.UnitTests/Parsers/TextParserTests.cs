using System;
using System.Collections.Generic;
using Xunit;
using MinimalCover.Infrastructure.Parsers;
using MinimalCover.Application.Parsers;
using MinimalCover.Domain.Models;
using MinimalCover.UnitTests.Utils;

namespace MinimalCover.Infrastructure.UnitTests.Parsers
{
  /// <summary>
  /// Explicitly give all the separators to <see cref="TextParser"/>
  /// because if the default arguments ever change in the future,
  /// these tests won't break because we're explicitly overriding the defaults
  /// </summary>
  public class TextParserTests
  {
    /// <summary>
    /// This class is only used to stored data for testing purposes.
    /// </summary>
    public class ParsedTextFdsTestData
    {
      public string Value { get; set; }

      public ISet<FunctionalDependency> ExpectedFds { get; set; }

      public string AttributeSeparator { get; set; }

      public string FdSeparator { get; set; }

      public string LeftRightSeparator { get; set; }
    }

    public static TheoryData<ParsedTextFdsTestData> ParsedTextTheoryData =
      new TheoryData<ParsedTextFdsTestData>()
      {
        new ParsedTextFdsTestData()
        {
          AttributeSeparator = ",",
          FdSeparator = ";",
          LeftRightSeparator = "-->",
          Value = "A-->B",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A", "B", ",")
          }
        },

        new ParsedTextFdsTestData()
        {
          AttributeSeparator = ",",
          FdSeparator = ";",
          LeftRightSeparator = "-->",
          Value = "A,C-->B",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A,C", "B", ",")
          }
        },

        new ParsedTextFdsTestData()
        {
          AttributeSeparator = ",",
          FdSeparator = ";",
          LeftRightSeparator = "-->",
          Value = "A,C-->B,D;E-->H",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A,C", "B,D", ","),
            FuncDepUtils.ConstructFdFromString("E", "H", ",")
          }
        },

        new ParsedTextFdsTestData()
        {
          AttributeSeparator = ",",
          FdSeparator = ";",
          LeftRightSeparator = "-->",
          Value = $"A,C-->B,D;E-->H;A,J-->H;{Environment.NewLine}B-->J;",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A,C", "B,D", ","),
            FuncDepUtils.ConstructFdFromString("E", "H", ","),
            FuncDepUtils.ConstructFdFromString("A,J", "H", ","),
            FuncDepUtils.ConstructFdFromString("B", "J", ",")
          }
        }
      };

    [Theory]
    // 1 null argument
    [InlineData(",", ";", null)]
    [InlineData(",", null, "-->")]
    [InlineData(null, ";", "-->")]
    // 2 null arguments
    [InlineData(null, null, "-->")]
    [InlineData(null, ";", null)]
    [InlineData(",", null, null)]
    // 3 null arguments
    [InlineData(null, null, null)]
    // 1 empty string argument
    public void Constructor_NullArguments_ThrowsArgumentException(string attrbSep, string fdSep, string leftRightSep)
    {
      Assert.Throws<ArgumentException>(() => new TextParser(attrbSep, fdSep, leftRightSep));
    }

    [Theory]
    // 1 empty string argument
    [InlineData(",", ";", "")]
    [InlineData(",", "", "-->")]
    [InlineData("", ";", "-->")]
    // 2 empty strings arguments
    [InlineData("", "", "-->")]
    [InlineData("", ";", "")]
    [InlineData(",", "", "")]
    // 3 empty strings arguments
    [InlineData("", "", "")]
    public void Constructor_EmptyStringArguments_ThrowsArgumentException(string attrbSep, string fdSep, string leftRightSep)
    {
      Assert.Throws<ArgumentException>(() => new TextParser(attrbSep, fdSep, leftRightSep));
    }

    [Fact]
    public void Format_SimpleGet_ReturnsTextFormat()
    {
      var textParser = new TextParser();
      Assert.Equal(ParseFormat.Text, textParser.Format);
    }

    [Theory]
    [InlineData("A-->", ",", ";", "-->")]
    [InlineData("-->B", ",", ";", "-->")]
    [InlineData("-->", ",", ";", "-->")]
    [InlineData("A-->B;-->D", ",", ";", "-->")]
    [InlineData("A, D-->B;E,H,J-->", ",", ";", "-->")]
    public void Parse_EmptyLhsOrRhs_ThrowsParserException(string value, string badAttrbSep, string fdSep, string leftRightSep)
    {
      IParser textParser = new TextParser(badAttrbSep, fdSep, leftRightSep);
      var ex = Assert.Throws<ParserException>(() => textParser.Parse(value));
      Assert.Contains(TextParser.EmptyLhsOrRhsMessage, ex.Message);
    }

    [Theory]
    [InlineData("A-->B", ",", ";", "x")]
    [InlineData("A-->B", ",", ";", "oh_no")]
    [InlineData("A-->B;C-->D", ",", ";", "bad")]
    [InlineData("A, D-->B;E,H, J-->D", ",", ";", "xxx")]
    public void Parse_BadLhsRhsSep_ThrowsParserException(string value, string attrbSep, string fdSep, string badLeftRightSep)
    {
      IParser textParser = new TextParser(attrbSep, fdSep, badLeftRightSep);
      var ex = Assert.Throws<ParserException>(() => textParser.Parse(value));
      Assert.Contains("must be separated by", ex.Message);
    }

    [Theory]
    [MemberData(nameof(ParsedTextTheoryData))]
    public void Parse_ValidString_ReturnsExpectedFdSet(ParsedTextFdsTestData testData)
    {
      IParser textParser = new TextParser(testData.AttributeSeparator,
                                          testData.FdSeparator, 
                                          testData.LeftRightSeparator);
      var parsedFds = textParser.Parse(testData.Value);
      Assert.Equal(testData.ExpectedFds, parsedFds);
    }

  }
}
