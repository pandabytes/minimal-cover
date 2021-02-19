using MinimalCover.Infrastructure.Parsers.Text;
using MinimalCover.Application.Parsers;

using Xunit;

namespace MinimalCover.Infrastructure.UnitTests.Parsers.Text
{
  /// <summary>
  /// Explicitly give all the separators to <see cref="TextParser"/>
  /// because if the default arguments ever change in the future,
  /// these tests won't break because we're explicitly overriding the defaults
  /// </summary>
  public class DefaultTextParserTests : TextParserTests
  {
    public override void Parse_EmptyLhsOrRhs_ThrowsParserException(string value, string badAttrbSep, string fdSep, string leftRightSep)
    {
      var textParser = new DefaultTextParser(badAttrbSep, fdSep, leftRightSep);
      var ex = Assert.Throws<ParserException>(() => textParser.Parse(value));
      Assert.Contains(TextParser.EmptyLhsOrRhsMessage, ex.Message);
    }

    public override void Format_SimpleGet_ReturnsTextFormat()
    {
      IParser textParser = new DefaultTextParser(",", ";", "-->");
      Assert.Equal(ParseFormat.Text, textParser.Format);
    }

    public override void Parse_BadLhsRhsSep_ThrowsParserException(string value, string attrbSep, string fdSep, string badLeftRightSep)
    {
      var textParser = new DefaultTextParser(attrbSep, fdSep, badLeftRightSep);
      var ex = Assert.Throws<ParserException>(() => textParser.Parse(value));
      Assert.Contains("must be separated by", ex.Message);
    }

    public override void Parse_ValidString_ReturnsExpectedFdSet(ParsedTextFdsTestData testData)
    {
      var textParser = new DefaultTextParser(testData.AttributeSeparator,
                                             testData.FdSeparator,
                                             testData.LeftRightSeparator);
      var parsedFds = textParser.Parse(testData.Value);
      Assert.Equal(testData.ExpectedFds, parsedFds);
    }

  }
}
