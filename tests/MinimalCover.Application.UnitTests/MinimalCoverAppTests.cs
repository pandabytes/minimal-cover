using System;
using System.IO;

using MinimalCover.Application.Algorithms;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Parsers.Settings;
using MinimalCover.UnitTests.Utils;
using static MinimalCover.UnitTests.Utils.ConfigurationUtils;

using Microsoft.Extensions.DependencyInjection;

using Moq;
using Xunit;

namespace MinimalCover.Application.UnitTests
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
          InputFilePath = @"..\..\..\SampleInputFiles\fds_1.json",
          ExpectedFilePath = @"..\..\..\SampleInputFiles\fds_1_expected.json",
          Format = ParseFormat.Json
        },
        new FdFileTestData()
        {
          InputFilePath = @"..\..\..\SampleInputFiles\fds_2.json",
          ExpectedFilePath = @"..\..\..\SampleInputFiles\fds_2_expected.json",
          Format = ParseFormat.Json
        },
        new FdFileTestData()
        {
          InputFilePath = @"..\..\..\SampleInputFiles\fds_3.json",
          ExpectedFilePath = @"..\..\..\SampleInputFiles\fds_3_expected.json",
          Format = ParseFormat.Json
        },
        new FdFileTestData()
        {
          InputFilePath = @"..\..\..\SampleInputFiles\fds_1.txt",
          ExpectedFilePath = @"..\..\..\SampleInputFiles\fds_1_expected.txt",
          Format = ParseFormat.Text
        },
        new FdFileTestData()
        {
          InputFilePath = @"..\..\..\SampleInputFiles\fds_2.txt",
          ExpectedFilePath = @"..\..\..\SampleInputFiles\fds_2_expected.txt",
          Format = ParseFormat.Text
        },
        new FdFileTestData()
        {
          InputFilePath = @"..\..\..\SampleInputFiles\fds_3.txt",
          ExpectedFilePath = @"..\..\..\SampleInputFiles\fds_3_expected.txt",
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
        SchemaFilePath = @"Parsers\Json\fd-schema.json"
      };

      var config = CreateConfig(textParserSettings, TextParserSettings.SectionPath)
                    .UpdateConfig(jsonParserSettings, JsonParserSettings.SectionPath);

      m_dp = new DependencyInjection(config);
      var alg = m_dp.Provider.GetRequiredService<IMinimalCover>();
      m_app = new MinimalCoverApp(alg);
    }

    private IParser GetParser(ParseFormat format)
    {
      switch (format)
      {
        case ParseFormat.Text:
          return m_dp.Provider.GetRequiredService<TextParser>();
        case ParseFormat.Json:
          return m_dp.Provider.GetRequiredService<JsonParser>();
        default:
          throw new NotSupportedException($"Format {format} is not supported yet");
      }
    }

    [Theory]
    [MemberData(nameof(FdFileTheoryData))]
    public void FindMinimalCover_StringArgument_ReturnsExpctedFds(FdFileTestData testData)
    {
      // Get the file content
      var value = File.ReadAllText(testData.InputFilePath);
      var expectedValue = File.ReadAllText(testData.ExpectedFilePath);

      // Get the approriate parser
      IParser parser = GetParser(testData.Format);
      
      // Find the minimal cover and validate the result
      var actualMinimalCover = m_app.FindMinimalCover(value, parser);
      var expectedMinimalCover = m_app.FindMinimalCover(expectedValue, parser);
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
      IParser parser = GetParser(testData.Format);

      // Find the minimal cover and validate the result
      var fds = parser.Parse(value);
      var actualMinimalCover = m_app.FindMinimalCover(fds); // Test this method 
      var expectedMinimalCover = parser.Parse(expectedValue);
      Assert.True(actualMinimalCover.SetEquals(expectedMinimalCover), "Minimal covers are not equal");
    }

  }
}
