using System;
using System.Collections.Generic;
using Xunit;
using MinimalCover.Core;
using MinimalCover.Core.Parsers;
using MinimalCover.Xunit.Core.Data;

namespace MinimalCover.Xunit.Core.Parsers
{
  public class YamlParserTests
  {
    public static TheoryData<ParsedFdsTestData> ParseTheoryData =
      new TheoryData<ParsedFdsTestData>()
      {
        new ParsedFdsTestData()
        {
          Value = @"
                  ---
                  - left:
                      - A
                      - B
                    right:
                      - C",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A,B", "C")
          }
        },
        new ParsedFdsTestData()
        {
          Value = @"
                  ---
                  - left:
                      - A
                      - B
                    right:
                      - C
                  - left:
                      - D
                      - E
                    right:
                      - G
                      - F",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A,B", "C"),
            new FunctionalDependency("D,E", "G,F")
          }
        },
        new ParsedFdsTestData()
        {
          Value = "---\r\n- left:\r\n    - A\r\n    - B\r\n  right:\r\n    - D\r\n",
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A, B", "D")
          }
        }
      };

    [Theory]
    [MemberData(nameof(ParseTheoryData))]
    public void Parse_ValidString_ReturnsFdSet(ParsedFdsTestData testData)
    {
      var fdsSet = YamlParser.Parse(testData.Value);
      Assert.Equal(testData.ExpectedFds, fdsSet);
    }

    [Theory]
    [InlineData(@"
                ---
                - leftx:
                    - A
                  right:
                    - B")]
    [InlineData(@"
                ---
                - left:
                    - A
                  rightx:
                    - B")]
    [InlineData(@"
                ---
                - leftx:
                    - A
                  rightx:
                    - B")]
    [InlineData(@"
                ---
                - left:
                    - A
                  right:
                    - B
                - left:
                    - A")]
    public void Parse_MissingProperties_ThrowsException(string value)
    {
      var ex = Assert.Throws<ArgumentException>(() => YamlParser.Parse(value));
      Assert.Contains(YamlParser.MissingPropertiesMessage, ex.Message);
    }

    [Theory]
    [InlineData(@"
                ---
                - left: 0
                  right:
                    - B")]
    [InlineData(@"
                ---
                - left:
                    - A
                  right: hello")]
    [InlineData(@"
                ---
                - left: 0
                  right: 0")]
    [InlineData(@"
                ---
                - left:
                    - A
                  right:
                    - B
                - left: 0
                  right: 0")]
    public void Parse_NonYamlList_ThrowsException(string value)
    {
      var ex = Assert.Throws<ArgumentException>(() => YamlParser.Parse(value));
      Assert.Contains(YamlParser.NonYamlListMessage, ex.Message);
    }

    [Theory]
    [InlineData(@"
                ---
                - left: []
                  right:
                    - B")]
    [InlineData(@"
                ---
                - left:
                    - A
                  right: []")]
    [InlineData(@"
                ---
                - left: []
                  right: []")]
    [InlineData(@"
                ---
                - left:
                    - A
                  right:
                    - B
                - left: []
                  right: []")]
    public void Parse_EmptyLhsOrRhs_ThrowsException(string value)
    {
      var ex = Assert.Throws<ArgumentException>(() => YamlParser.Parse(value));
      Assert.Contains(YamlParser.EmptyLhsOrRhsMessage, ex.Message);
    }
  }
}
