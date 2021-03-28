using System;
using System.Reflection;
using System.Collections.Generic;

using MinimalCover.Application.Parsers;
using MinimalCover.Application.Parsers.Settings;
using MinimalCover.Domain.Models;
using MinimalCover.UnitTests.Utils;
using static MinimalCover.UnitTests.Utils.ConfigurationUtils;

using Microsoft.Extensions.DependencyInjection;

using Moq;
using Xunit;

namespace MinimalCover.Application.UnitTests.Parsers.Text
{
  public class TextParserTests
  {
    /// <summary>
    /// This class is only used to stored data for testing purposes.
    /// </summary>
    public class ParsedTextFdsTestData
    {
      public string Value { get; set; } = null!;

      public ISet<FunctionalDependency> ExpectedFds { get; set; } = null!;

      public string AttributeSeparator { get; set; } = null!;

      public string FdSeparator { get; set; } = null!;

      public string LeftRightSeparator { get; set; } = null!;
    }

    public static readonly TheoryData<ParsedTextFdsTestData> ParsedTextTheoryData =
      new ()
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
    [InlineData("", ";", "-->")]
    [InlineData(" ", ";", "-->")]
    [InlineData(null, ";", "-->")]
    [InlineData(",", "", "-->")]
    [InlineData(",", " ", "-->")]
    [InlineData(",", null, "-->")]
    [InlineData(",", ";", "")]
    [InlineData(",", ";", " ")]
    [InlineData(",", ";", null)]
    public void Constructor_BadSeparators_ThrowArgumentException(string attributeSep, string fdSep, string leftRightSep)
    {
      var settings = new TextParserSettings 
      { 
        AttributeSeparator = attributeSep, 
        FdSeparator = fdSep, 
        LeftRightSeparator = leftRightSep 
      };

      // Mock the abstract class so that we can use its constructor
      // The outer exception is thrown by Mock and the actual
      // exception of our code is the inner exception
      var ex = Assert.Throws<TargetInvocationException>(() => new Mock<TextParser>(settings).Object);
      var actualEx = ex.InnerException;

      Assert.IsType<ArgumentException>(actualEx);
    }

    [Theory]
    [InlineData("A-->", ",", ";", "-->")]
    [InlineData("-->B", ",", ";", "-->")]
    [InlineData("-->", ",", ";", "-->")]
    [InlineData("A-->B;-->D", ",", ";", "-->")]
    [InlineData("A, D-->B;E,H,J-->", ",", ";", "-->")]
    public void Parse_EmptyLhsOrRhs_ThrowsParserException(string value, string badAttrbSep, string fdSep, string leftRightSep)
    {
      var textParser = GetTextParser(badAttrbSep, fdSep, leftRightSep);
      var ex = Assert.Throws<ParserException>(() => textParser.Parse(value));
      Assert.Contains(TextParser.EmptyLhsOrRhsMessage, ex.Message);
    }

    [Fact]
    public void Format_SimpleGet_ReturnsTextFormat()
    {
      IParser textParser = GetTextParser(",", ";", "-->");
      Assert.Equal(ParseFormat.Text, textParser.Format);
    }

    [Theory]
    [InlineData("A-->B", ",", ";", "x")]
    [InlineData("A-->B", ",", ";", "oh_no")]
    [InlineData("A-->B;C-->D", ",", ";", "bad")]
    [InlineData("A, D-->B;E,H, J-->D", ",", ";", "xxx")]
    public void Parse_BadLhsRhsSep_ThrowsParserException(string value, string attrbSep, string fdSep, string badLeftRightSep)
    {
      var textParser = GetTextParser(attrbSep, fdSep, badLeftRightSep);
      var ex = Assert.Throws<ParserException>(() => textParser.Parse(value));
      Assert.Contains("must be separated by", ex.Message);
    }

    [Theory]
    [MemberData(nameof(ParsedTextTheoryData))]
    public void Parse_ValidString_ReturnsExpectedFdSet(ParsedTextFdsTestData testData)
    {
      var textParser = GetTextParser(testData.AttributeSeparator,
                                             testData.FdSeparator,
                                             testData.LeftRightSeparator);
      var parsedFds = textParser.Parse(testData.Value);
      Assert.Equal(testData.ExpectedFds, parsedFds);
    }

    /// <summary>
    /// Get the text parser via dependency injection
    /// </summary>
    /// <param name="attrbSep">Attribute separator</param>
    /// <param name="fdSep">Functional dependency separator</param>
    /// <param name="leftRightSep">Separtor between LHS and RHS</param>
    /// <returns>The text parser</returns>
    protected static TextParser GetTextParser(string attrbSep, string fdSep, string leftRightSep)
    {
      var settings = new TextParserSettings { AttributeSeparator = attrbSep, FdSeparator = fdSep, LeftRightSeparator = leftRightSep };
      var config = CreateConfig(settings, TextParserSettings.SectionPath);
      var dp = new DependencyInjection(config);
      return dp.Provider.GetRequiredService<TextParser>();
    }
  }


}
