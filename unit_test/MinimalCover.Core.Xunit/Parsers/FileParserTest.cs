using System;
using System.Reflection;
using Xunit;
using Moq;

namespace MinimalCover.Core.Xunit.Parsers
{
  public class FileParserTest
  {
    [Theory]
    [InlineData("test")]
    [InlineData(null)]
    [InlineData(@"C:\Users\temp\Dekstop\fds.txt")]
    [InlineData("~/Dekstop/fds.txt")]
    public void Constructor_InvalidPath_Test(string filePath)
    {
      var ex = Assert.Throws<TargetInvocationException>(() => new Mock<Core.Parsers.FileParser>(filePath).Object);
      Assert.IsType<ArgumentException>(ex.InnerException);
    }

    [Fact]
    public void Constructor_ValidPath_Test()
    {
      var assemblyPath = Assembly.GetExecutingAssembly().Location;
      var fileParser = new Mock<Core.Parsers.FileParser>(assemblyPath).Object;
      Assert.Equal(fileParser.FilePath, assemblyPath);
    }
  }
}
