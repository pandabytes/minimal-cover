using System;
using System.Reflection;

using MinimalCover.Application.Parsers;
using MinimalCover.Infrastructure.Parsers.Json.Converter;

using Xunit;
using Moq;

namespace MinimalCover.Infrastructure.UnitTests.Parsers.Json
{
  /// <summary>
  /// Currently, the test data rely on the schema defined in
  /// "fd-schema.json"
  /// </summary>
  public class JsonConverterParserTests : JsonParserTests
  {
    /// <summary>
    /// Constructor
    /// </summary>
    public JsonConverterParserTests()
    {
      m_jsonParser = GetJsonParser(@"Parsers\Json\fd-schema.json");
    }

    public override void Constructor_InvalidArguments_ThrowsArgumentException()
    {
      // Mock the abstract class so that we can use its constructor
      // The outer exception is thrown by Mock and the actual
      // exception of our code is the inner exception
      var ex = Assert.Throws<TargetInvocationException>(() => new Mock<JsonParser>(null).Object);
      var actualEx = ex.InnerException;

      Assert.IsType<ArgumentNullException>(actualEx);
    }

    public override void Format_SimpleGet_ReturnsJsonFormat()
    {
      Assert.Equal(ParseFormat.Json, ((IParser)m_jsonParser).Format);
    }

    public override void Parse_InvalidJsonString_ThrowsParserException(string value)
    {
      Assert.Throws<ParserException>(() => m_jsonParser.Parse(value));
    }
    
    public override void Parse_ValidString_ReturnsExpectedFdSet(ParsedJsonFdsTestData testData)
    {
      var parsedFds = m_jsonParser.Parse(testData.Value);
      Assert.Equal(testData.ExpectedFds, parsedFds);
    }

  }
}
