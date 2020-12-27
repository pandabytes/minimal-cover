using System;
using System.Collections.Generic;
using Xunit;

namespace MinimalCover.Core.Parsers.Xunit
{
  public class TextParserTests
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
          Value = $"A{TextParser.LeftRightSeparator}B{TextParser.FdSeparator}",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A", "B")
          }
        },
        new ParsedFdsTestData()
        {
          Value = $"A{TextParser.LeftRightSeparator}B{TextParser.FdSeparator}" + 
                  $"C{TextParser.LeftRightSeparator}D",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A", "B"),
            new FunctionalDependency("C", "D")
          }
        },
        new ParsedFdsTestData()
        {
          Value = $"A{TextParser.AttributeSeparator}E{TextParser.LeftRightSeparator}B{TextParser.FdSeparator}" +
                  $"C{TextParser.LeftRightSeparator}D{TextParser.FdSeparator}" +
                  $"A{TextParser.AttributeSeparator}F{TextParser.LeftRightSeparator}D{TextParser.FdSeparator}",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency($"A{TextParser.AttributeSeparator}E", "B", TextParser.AttributeSeparator),
            new FunctionalDependency("C", "D"),
            new FunctionalDependency($"A{TextParser.AttributeSeparator}F", "D", TextParser.AttributeSeparator)
          }
        }
      };

    public static TheoryData<string> BadLhsRhsSepTheoryData =
      new TheoryData<string>()
      {
        "A-x->B",
        $"A{TextParser.LeftRightSeparator}B;A-C",
        "A->B;A-C"
      };
    
    public static TheoryData<string> EmptyLhsOrRhsTheoryData =
      new TheoryData<string>()
      {
        $"A{TextParser.LeftRightSeparator}",
        $"{TextParser.LeftRightSeparator}B",
        $"A{TextParser.LeftRightSeparator}B{TextParser.FdSeparator}" +
          $"A{TextParser.LeftRightSeparator}",
        $"A{TextParser.LeftRightSeparator}B{TextParser.FdSeparator}" + 
          $"A{TextParser.LeftRightSeparator}C{TextParser.FdSeparator}" + 
          $"{TextParser.LeftRightSeparator}B"
      };

    [Theory]
    [MemberData(nameof(BadLhsRhsSepTheoryData))]
    public void Parse_BadLhsRhsSep_ThrowsArgumentException(string value)
    {
      var ex = Assert.Throws<ArgumentException>(() => TextParser.Parse(value));
      Assert.Equal(TextParser.BadLhsRhsSepMessage, ex.Message);
    }

    [Theory]
    [MemberData(nameof(EmptyLhsOrRhsTheoryData))]
    public void Parse_EmptyLhsOrRhs_ThrowsArgumentException(string value)
    {
      var ex = Assert.Throws<ArgumentException>(() => TextParser.Parse(value));
      Assert.Equal(TextParser.EmptyLhsOrRhsMessage, ex.Message);
    }

    [Theory]
    [MemberData(nameof(ParseTheoryData))]
    public void Parse_ValidString_ReturnsFdSet(ParsedFdsTestData testData)
    {
      var fdsSet = TextParser.Parse(testData.Value);
      Assert.Equal(testData.ExpectedFds, fdsSet);
    }
  }
}
