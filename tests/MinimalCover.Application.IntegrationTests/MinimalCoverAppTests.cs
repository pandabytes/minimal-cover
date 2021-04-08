using System;
using System.IO;
using System.Collections.Generic;

using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Parsers.Settings;
using MinimalCover.Tests.Utils;
using static MinimalCover.Tests.Utils.ConfigurationUtils;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace MinimalCover.Application.IntegrationTests
{
  public class MinimalCoverAppTests
  {
    /// <summary>
    /// Containing the input and expected file of
    /// functional dependencies
    /// </summary>
    public class FdFileTestData
    {
      public string InputFilePath { get; set; } = null!;

      public string ExpectedFilePath { get; set; } = null!;

      public ParseFormat Format { get; set; }
    }

    public static readonly TheoryData<FdFileTestData> FdFileTheoryData =
      new()
      {
        new FdFileTestData()
        {
          InputFilePath = "../../../SampleInputFiles/fds_1.json",
          ExpectedFilePath = "../../../SampleInputFiles/fds_1_expected.json",
          Format = ParseFormat.Json
        },
        new FdFileTestData()
        {
          InputFilePath = "../../../SampleInputFiles/fds_2.json",
          ExpectedFilePath = "../../../SampleInputFiles/fds_2_expected.json",
          Format = ParseFormat.Json
        },
        new FdFileTestData()
        {
          InputFilePath = "../../../SampleInputFiles/fds_3.json",
          ExpectedFilePath = "../../../SampleInputFiles/fds_3_expected.json",
          Format = ParseFormat.Json
        },
        new FdFileTestData()
        {
          InputFilePath = "../../../SampleInputFiles/fds_1.txt",
          ExpectedFilePath = "../../../SampleInputFiles/fds_1_expected.txt",
          Format = ParseFormat.Text
        },
        new FdFileTestData()
        {
          InputFilePath = "../../../SampleInputFiles/fds_2.txt",
          ExpectedFilePath = "../../../SampleInputFiles/fds_2_expected.txt",
          Format = ParseFormat.Text
        },
        new FdFileTestData()
        {
          InputFilePath = "../../../SampleInputFiles/fds_3.txt",
          ExpectedFilePath = "../../../SampleInputFiles/fds_3_expected.txt",
          Format = ParseFormat.Text
        }
      };

    /// <summary>
    /// Object under test
    /// </summary>
    private readonly MinimalCoverApp m_app;

    /// <summary>
    /// Dependency injection object
    /// </summary>
    private readonly DependencyInjection m_dp;

    /// <summary>
    /// Constructor
    /// </summary>
    public MinimalCoverAppTests()
    {
      var textParserSettings = new TextParserSettings 
      { 
        AttributeSeparator = ",", 
        FdSeparator = ";", 
        LeftRightSeparator = "-->" 
      };

      var jsonParserSettings = new JsonParserSettings
      {
        SchemaFilePath = "Parsers/Json/fd-schema.json"
      };

      var config = CreateConfig(textParserSettings, TextParserSettings.SectionPath)
                    .UpdateConfig(jsonParserSettings, JsonParserSettings.SectionPath);

      m_dp = new DependencyInjection(config);
      m_app = m_dp.Provider.GetRequiredService<MinimalCoverApp>();
    }

    [Theory]
    [MemberData(nameof(FdFileTheoryData))]
    public void FindMinimalCover_StringArgument_ReturnsExpctedFds(FdFileTestData testData)
    {
      // Get the file content
      var value = File.ReadAllText(testData.InputFilePath);
      var expectedValue = File.ReadAllText(testData.ExpectedFilePath);

      // Get the approriate parser
      IParser parser = m_dp.Provider.GetRequiredService<GetParser>()(testData.Format);
      
      // Find the minimal cover and validate the result
      var actualMinimalCover = m_app.FindMinimalCover(parser, value);
      var expectedMinimalCover = m_app.FindMinimalCover(parser, expectedValue);
      Assert.True(actualMinimalCover.SetEquals(expectedMinimalCover), "Minimal covers are not equal");
    }

    [Theory]
    [MemberData(nameof(FdFileTheoryData))]
    public void FindMinimalCover_FuncDepArgument_ReturnsExpctedFds(FdFileTestData testData)
    {
      // Get the file content
      var value = File.ReadAllText(testData.InputFilePath);
      var expectedValue = File.ReadAllText(testData.ExpectedFilePath);

      // Get the approriate parser
      IParser parser = m_dp.Provider.GetRequiredService<GetParser>()(testData.Format);

      // Find the minimal cover and validate the result
      var fds = parser.Parse(value);
      var actualMinimalCover = m_app.FindMinimalCover(fds); // Test this method 
      var expectedMinimalCover = parser.Parse(expectedValue);
      Assert.True(actualMinimalCover.SetEquals(expectedMinimalCover), "Minimal covers are not equal");
    }

    [Fact]
    public void FindMinimalCover_OneFuncDepWithSameLeftRight_ReturnsEmpty()
    {
      var fds = new HashSet<FunctionalDependency> {
        FuncDepUtils.ConstructFdFromString("a", "a", ",")
      };

      var mc = m_app.FindMinimalCover(fds);
      Assert.Empty(mc);
    }

    [Fact]
    public void FindMinimalCover_MultipleFuncDepsWithSameLeftRight_ReturnsEmpty()
    {
      var fds = new HashSet<FunctionalDependency> {
        FuncDepUtils.ConstructFdFromString("a", "a", ","),
        FuncDepUtils.ConstructFdFromString("b", "b", ","),
        FuncDepUtils.ConstructFdFromString("c", "c", ",")
      };

      var mc = m_app.FindMinimalCover(fds);
      Assert.Empty(mc);
    }

  }
}
