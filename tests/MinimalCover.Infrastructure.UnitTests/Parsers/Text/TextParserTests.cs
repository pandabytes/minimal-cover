using System;
using System.Collections.Generic;
using System.Reflection;

using MinimalCover.Infrastructure.Parsers.Text;
using MinimalCover.Application.Parsers;
using MinimalCover.Domain.Models;
using MinimalCover.UnitTests.Utils;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using NSubstitute;

using Xunit;

namespace MinimalCover.Infrastructure.UnitTests.Parsers.Text
{
  /// <summary>
  /// Explicitly give all the separators to <see cref="TextParser"/>
  /// because if the default arguments ever change in the future,
  /// these tests won't break because we're explicitly overriding the defaults
  /// </summary>
  /// <remarks>
  /// This class tests the provided <see cref="TextParser"/> implementation
  /// from <see cref="DependencyInjection"/>
  /// </remarks>
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

    /// <summary>
    /// Get a text parser object under test, provided with different
    /// configurations
    /// </summary>
    /// <param name="dict">
    /// Dictionary containing the following keys:
    ///  "textParser:attrbSep"
    ///  "textParser:fdSep"
    ///  "textParser:leftRightSep"
    /// </param>
    /// <returns>Text parser object</returns>
    private TextParser GetTextParser(IDictionary<string, string> dict)
    {
      var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(dict)
                    .Build();

      var services = new ServiceCollection();
      services.AddInfrastructure(config);
      var provider = services.BuildServiceProvider();

      var getParser = provider.GetService<GetParser>();
      return (TextParser)getParser(ParseFormat.Text);
    }

    [Theory]
    // Invalid argument(s) is/are empty string(s)
    [InlineData(",", ";", "")]
    [InlineData(",", "", "-->")]
    [InlineData("", ";", "-->")]
    [InlineData("", "", "-->")]
    [InlineData("", ";", "")]
    [InlineData(",", "", "")]
    [InlineData("", "", "")]
    // Invalid argument(s) is/are null
    [InlineData(",", ";", null)]
    [InlineData(",", null, "-->")]
    [InlineData(null, ";", "-->")]
    [InlineData(null, null, "-->")]
    [InlineData(null, ";", null)]
    [InlineData(",", null, null)]
    [InlineData(null, null, null)]
    public void Constructor_InvalidArguments_ThrowsArgumentException(string attrbSep, string fdSep, string leftRightSep)
    {
      // Mock the abstract class so that we can use its constructor
      // The outer exception is thrown by NSubstitute and the actual
      // exception of our code is the inner exception
      var ex = Assert.Throws<TargetInvocationException>(() => Substitute.For<TextParser>(attrbSep, fdSep, leftRightSep));
      var actualEx = ex.InnerException;

      Assert.IsType<ArgumentException>(actualEx);
      Assert.Equal(TextParser.InvalidSepsMessage, actualEx.Message);
    }

    [Fact]
    public void Format_SimpleGet_ReturnsTextFormat()
    {
      var dict = new Dictionary<string, string>()
      {
        { "textParser:attrbSep", "," },
        { "textParser:fdSep", ";" },
        { "textParser:leftRightSep", "-->" }
      };
      IParser textParser = GetTextParser(dict);
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
      var dict = new Dictionary<string, string>()
      {
        { "textParser:attrbSep", badAttrbSep },
        { "textParser:fdSep", fdSep },
        { "textParser:leftRightSep", leftRightSep }
      };
      IParser textParser = GetTextParser(dict);
      
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
      var dict = new Dictionary<string, string>()
      {
        { "textParser:attrbSep", attrbSep },
        { "textParser:fdSep", fdSep },
        { "textParser:leftRightSep", badLeftRightSep }
      };
      IParser textParser = GetTextParser(dict);

      var ex = Assert.Throws<ParserException>(() => textParser.Parse(value));
      Assert.Contains("must be separated by", ex.Message);
    }

    [Theory]
    [MemberData(nameof(ParsedTextTheoryData))]
    public void Parse_ValidString_ReturnsExpectedFdSet(ParsedTextFdsTestData testData)
    {
      var dict = new Dictionary<string, string>()
      {
        { "textParser:attrbSep", testData.AttributeSeparator },
        { "textParser:fdSep", testData.FdSeparator },
        { "textParser:leftRightSep", testData.LeftRightSeparator }
      };
      IParser textParser = GetTextParser(dict);

      var parsedFds = textParser.Parse(testData.Value);
      Assert.Equal(testData.ExpectedFds, parsedFds);
    }

  }
}
