using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;

namespace MinimalCover.Core.Xunit.Parsers.Text
{
  public class TextParserTest
  {
    public class TestData
    {
      public string FilePath { get; set; }

      public ISet<FunctionalDependency> ExpectedFds { get; set; }
    }

    public static TheoryData<TestData> GoodTheoryData =
      new TheoryData<TestData>
      {
        new TestData
        {
          FilePath = @".\Parsers\TestData\GoodData\fds_1.txt",
          ExpectedFds = new HashSet<FunctionalDependency>() {
            new FunctionalDependency("A", "D"),
            new FunctionalDependency("B,C", "A,D"),
            new FunctionalDependency("C", "B"),
            new FunctionalDependency("E", "A"),
            new FunctionalDependency("E", "D")
          } 
        },
        new TestData
        {
          FilePath = @".\Parsers\TestData\GoodData\fds_2.txt",
          ExpectedFds = new HashSet<FunctionalDependency>() {
            new FunctionalDependency("A", "B,C"),
            new FunctionalDependency("B", "C"),
            new FunctionalDependency("A,B", "D")
          }
        },
        new TestData
        {
          FilePath = @".\Parsers\TestData\GoodData\fds_3.txt",
          ExpectedFds = new HashSet<FunctionalDependency>() {
            new FunctionalDependency("A,B", "C"),
            new FunctionalDependency("C", "A"),
            new FunctionalDependency("B,C", "D"),
            new FunctionalDependency("A,C,D", "B,D"),
            new FunctionalDependency("D", "E"),
            new FunctionalDependency("D", "G"),
            new FunctionalDependency("B,E", "C"),
            new FunctionalDependency("C,G", "B"),
            new FunctionalDependency("C,G", "D"),
            new FunctionalDependency("C,E", "A"),
            new FunctionalDependency("C,E", "G")
          }
        }
    };

    public static TheoryData<TestData> BadLeftRightSepTheoryData =
      new TheoryData<TestData>
      {
        new TestData
        {
          FilePath = @".\Parsers\TestData\GoodData\fds_bad_sep_1.txt"
        },
        new TestData
        {
          FilePath = @".\Parsers\TestData\GoodData\fds_bad_sep_2.txt"
        },
        new TestData
        {
          FilePath = @".\Parsers\TestData\GoodData\fds_bad_sep_3.txt"
        }
    };

    [Theory]
    [MemberData(nameof(GoodTheoryData))]
    public void Parse_Test(TestData testData)
    {
      var textParser = new Core.Parsers.Text.TextFileParser(testData.FilePath);
      textParser.Parse();
      Assert.Equal(testData.ExpectedFds, textParser.ParsedFds);
    }

    [Theory]
    [MemberData(nameof(BadLeftRightSepTheoryData))]
    public void Parse_BadLeftRightSep_Test(TestData testData)
    {
      Assert.Throws<ArgumentException>(() => {
        var textParser = new Core.Parsers.Text.TextFileParser(testData.FilePath);
        textParser.Parse();
      });
    }

    //[Theory]
    //[MemberData(nameof(EmptyLeftRightTheoryData))]
    //public void Parse_EmptyLeftRight_Test(TestData testData)
    //{
    //  Assert.Throws<ArgumentException>(() =>
    //  {
    //    var cliParser = new Core.Parsers.Cli.CliParser(testData.FdsString);
    //    cliParser.Parse();
    //  });
    //}
  }
}
