using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MinimalCover.Xunit
{
  public class AttributeSetTest
  {
    [Theory]
    [InlineData("", ',')]
    [InlineData("a|b|c|d", '-')]
    [InlineData("a,b,c", ',')]
    [InlineData("a,b,c,d,e,f,g,h,i,j,k", ',')]
    [InlineData("a|b|c|d", '|')]
    [InlineData("a-b-c-d-e-1-2-3-5", '-')]
    public void String_Constructor_Test(string setStr, char separator)
    {
      var tokens = setStr.Split(separator);
      var attributeSet = new AttributeSet(setStr, separator);
      Assert.True(tokens.Length == attributeSet.Count, $"Set doesn't have {tokens.Length} items");
    }
  }
}
