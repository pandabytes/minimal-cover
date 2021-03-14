using MinimalCover.Application.Parsers;

using Xunit;

namespace MinimalCover.Infrastructure.UnitTests.Parsers.Text
{
  public class DefaultTextParserTests : TextParserTests
  {
    public override void Parse_EmptyLhsOrRhs_ThrowsParserException(string value, string badAttrbSep, string fdSep, string leftRightSep)
    {
      var textParser = GetTextParser(badAttrbSep, fdSep, leftRightSep);
      var ex = Assert.Throws<ParserException>(() => textParser.Parse(value));
      Assert.Contains(TextParser.EmptyLhsOrRhsMessage, ex.Message);
    }

    public override void Format_SimpleGet_ReturnsTextFormat()
    {
      IParser textParser = GetTextParser(",", ";", "-->");
      Assert.Equal(ParseFormat.Text, textParser.Format);
    }

    public override void Parse_BadLhsRhsSep_ThrowsParserException(string value, string attrbSep, string fdSep, string badLeftRightSep)
    {
      var textParser = GetTextParser(attrbSep, fdSep, badLeftRightSep);
      var ex = Assert.Throws<ParserException>(() => textParser.Parse(value));
      Assert.Contains("must be separated by", ex.Message);
    }

    public override void Parse_ValidString_ReturnsExpectedFdSet(ParsedTextFdsTestData testData)
    {
      var textParser = GetTextParser(testData.AttributeSeparator,
                                             testData.FdSeparator,
                                             testData.LeftRightSeparator);
      var parsedFds = textParser.Parse(testData.Value);
      Assert.Equal(testData.ExpectedFds, parsedFds);
    }

  }
}
