using System;
using Xunit;

namespace MinimalCover.Application.UnitTests
{
  public class MinimalCoverAppTests
  {
    [Fact]
    public void Constructor_NullArgument_ThrowsArgumentNullException()
    {
      Assert.Throws<ArgumentNullException>(() => new MinimalCoverApp(null));
    }
  }
}
