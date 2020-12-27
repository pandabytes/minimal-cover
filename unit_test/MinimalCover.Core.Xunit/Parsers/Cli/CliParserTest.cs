using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;

namespace MinimalCover.Core.Xunit.Parsers.Cli
{
  public class CliParserTest : Core.Parsers.Cli.CliParser
  {
    public class TestData
    {
      public string FdsString { get; set; }

      public ISet<FunctionalDependency> ExpectedFds { get; set; }
    }

    public static TheoryData<TestData> GoodTheoryData =
      new TheoryData<TestData>
      {
        new TestData()
        {
          FdsString = $"A{LeftRightSeparator}B{FdSeparator}C{LeftRightSeparator}D",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A", "B"),
            new FunctionalDependency("C", "D")
          }
        },
        new TestData()
        {
          FdsString = $"A{LeftRightSeparator}B",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A", "B")
          }
        },
        new TestData()
        {
          FdsString = $"A{LeftRightSeparator}B{FdSeparator}C{LeftRightSeparator}D;A{LeftRightSeparator}X;",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A", "B"),
            new FunctionalDependency("C", "D"),
            new FunctionalDependency("A", "X"),
          }
        }
      };

    public static TheoryData<TestData> BadLeftRightSepTheoryData =
      new TheoryData<TestData>
      {
        new TestData()
        {
          FdsString = "A__>B{FdSeparator}C__>D"
        },
        new TestData()
        {
          FdsString = "A__>B"
        },
        new TestData()
        {
          FdsString = $"A__>B{FdSeparator}C__>D;A__>X;"
        }
      };

    public static TheoryData<TestData> EmptyLeftRightTheoryData =
      new TheoryData<TestData>
      {
        new TestData()
        {
          FdsString = $"A{LeftRightSeparator}{FdSeparator}B{LeftRightSeparator}C"
        },
        new TestData()
        {
          FdsString = $"{LeftRightSeparator}B"
        },
        new TestData()
        {
          FdsString = $"{LeftRightSeparator}"
        }
  };

    public override void Parse()
    {
      throw new NotImplementedException("Not implemented for unit test");
    }

    /// <summary>
    /// Needed for unit testing
    /// </summary>
    /// <param name="fdsString"></param>
    private CliParserTest(string fdsString) : base(fdsString) { }

    /// <summary>
    /// Needed for Xunit to construct this object
    /// </summary>
    public CliParserTest() : base(null) { }

    [Theory]
    [MemberData(nameof(GoodTheoryData))]
    public void Parse_Test(TestData testData)
    {
      var cliParser = new Core.Parsers.Cli.CliParser(testData.FdsString);
      cliParser.Parse();
      Assert.Equal(testData.ExpectedFds, cliParser.ParsedFds);
    }

    [Theory]
    [MemberData(nameof(BadLeftRightSepTheoryData))]
    public void Parse_BadLeftRightSep_Test(TestData testData)
    {
      Assert.Throws<ArgumentException>(() =>
      {
        var cliParser = new Core.Parsers.Cli.CliParser(testData.FdsString);
        cliParser.Parse();
      });
    }

    [Theory]
    [MemberData(nameof(EmptyLeftRightTheoryData))]
    public void Parse_EmptyLeftRight_Test(TestData testData)
    {
      Assert.Throws<ArgumentException>(() =>
      {
        var cliParser = new Core.Parsers.Cli.CliParser(testData.FdsString);
        cliParser.Parse();
      });
    }
  }
}
