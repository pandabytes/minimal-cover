using System;
using System.Reflection;

using MinimalCover.Application.Parsers;
using MinimalCover.Infrastructure.Parsers.Json.Converter;

using NSubstitute;
using Xunit;

namespace MinimalCover.Infrastructure.UnitTests.Parsers.Json
{
  /// <summary>
  /// Currently, the test data rely on the schema defined in
  /// <see cref="FdSetConverter.SchemaFilePath"/>
  /// </summary>
  public class JsonConverterParserTests : JsonParserTests
  {
    /// <summary>
    /// Constructor
    /// </summary>
    public JsonConverterParserTests()
    {
      var converter = new FdSetConverter();
      m_jsonParser = new JsonConverterParser(converter); 
    }

    public override void Constructor_InvalidArguments_ThrowsArgumentException(string schema)
    {
      // Mock the abstract class so that we can use its constructor
      // The outer exception is thrown by NSubstitute and the actual
      // exception of our code is the inner exception
      var ex = Assert.Throws<TargetInvocationException>(() => Substitute.For<JsonParser>(schema));
      var actualEx = ex.InnerException;

      Assert.IsType<ArgumentException>(actualEx);
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
