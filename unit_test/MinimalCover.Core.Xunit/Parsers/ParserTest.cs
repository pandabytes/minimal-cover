using System;
using System.Collections.Generic;
using Xunit;
using Moq;

namespace MinimalCover.Core.Xunit.Parsers
{
  public class ParserTest
  {
    [Fact]
    public void Constructor_ParseFds_Initialized_Test()
    {
      var mockParser = new Mock<Core.Parsers.Parser>();
      Assert.Empty(mockParser.Object.ParsedFds);
    }
  }
}
