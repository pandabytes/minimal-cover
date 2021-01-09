using Xunit;
using MinimalCover.Core;

namespace MinimalCover.Xunit.Core
{
  public class AttributeSetTests
  {
    [Theory]
    [InlineData("", ',')]
    [InlineData("a", ',')]
    [InlineData("a|b|c|d", '-')]
    [InlineData("a,b,c", ',')]
    [InlineData("a,b,c,d,e,f,g,h,i,j,k", ',')]
    [InlineData("a|b|c|d", '|')]
    [InlineData("a-b-c-d-e-1-2-3-5", '-')]
    public void Constructor_WithString_ReturnsAttributeSet(string setStr, char separator)
    {
      var tokens = setStr.Split(separator);
      var tokensStr = $"[{string.Join(',', tokens)}]";
      var attributeSet = new AttributeSet(setStr, separator);
      Assert.True(attributeSet.SetEquals(tokens), $"Set doesn't contain all attributes {tokensStr}");
    }
  }
}
