using System;
using System.Collections.Generic;

using MinimalCover.Application.Parsers;
using MinimalCover.Domain.Models;
using MinimalCover.UnitTests.Utils;

using Xunit;

namespace MinimalCover.Infrastructure.UnitTests.Parsers.Text
{
  /// <summary>
  /// This class encapsulates all the tests for any class
  /// that inhereit <see cref="TextParser"/>. 
  /// 
  /// To test an implementation of <see cref="TextParser"/>, simply
  /// inhereit <see cref="TextParserTests"/> and provide
  /// the implementation to the tests
  /// </summary>
  public abstract class TextParserTests
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
    [InlineData("A-->", ",", ";", "-->")]
    [InlineData("-->B", ",", ";", "-->")]
    [InlineData("-->", ",", ";", "-->")]
    [InlineData("A-->B;-->D", ",", ";", "-->")]
    [InlineData("A, D-->B;E,H,J-->", ",", ";", "-->")]
    public abstract void Parse_EmptyLhsOrRhs_ThrowsParserException(string value, string badAttrbSep, string fdSep, string leftRightSep);

    [Fact]
    public abstract void Format_SimpleGet_ReturnsTextFormat();

    [Theory]
    [InlineData("A-->B", ",", ";", "x")]
    [InlineData("A-->B", ",", ";", "oh_no")]
    [InlineData("A-->B;C-->D", ",", ";", "bad")]
    [InlineData("A, D-->B;E,H, J-->D", ",", ";", "xxx")]
    public abstract void Parse_BadLhsRhsSep_ThrowsParserException(string value, string attrbSep, string fdSep, string badLeftRightSep);

    [Theory]
    [MemberData(nameof(ParsedTextTheoryData))]
    public abstract void Parse_ValidString_ReturnsExpectedFdSet(ParsedTextFdsTestData testData);
  }


}
