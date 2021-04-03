using Xunit;
using System;
using System.Collections.Generic;
using MinimalCover.Domain.Models;
using MinimalCover.Tests.Utils;

namespace MinimalCover.Domain.UnitTests.Models
{
  public class FunctionalDependencyTests
  {
    [Theory]
    [InlineData("a", "a")]
    [InlineData("a,b", "b,a")]
    [InlineData("a,b,c", "b,a,c")]
    public void Constructor_LeftAndRightAreSameSets_ThrowsArgumentException(string left, string right)
    {
      var ex = Assert.Throws<ArgumentException>(() => FuncDepUtils.ConstructFdFromString(left, right, ","));
      Assert.Equal(FunctionalDependency.SameLeftRightMessage, ex.Message);
    }

    [Theory]
    [InlineData(new string[] { "a" }, new string[] { })]
    [InlineData(new string[] { }, new string[] { "a" })]
    public void Constructor_EmptyArguments_ThrowsArgumentException(string[] leftAttrbs, string[] rightAttrbs)
    {
      var left = new HashSet<string>(leftAttrbs);
      var right = new HashSet<string>(rightAttrbs);
      var ex = Assert.Throws<ArgumentException>(() => new FunctionalDependency(left, right));
      Assert.Equal(FunctionalDependency.NonEmptyLeftRightMessage, ex.Message);
    }

    [Theory]
    [InlineData(new string[] { " " }, new string[] { "   " })]
    [InlineData(new string[] { "a" }, new string[] { "" })]
    [InlineData(new string[] { "a", "b" }, new string[] { "  " })]
    [InlineData(new string[] { "  ", "b" }, new string[] { "a", "b" })]
    public void Constructor_NullAndEmptyAttributes_ThrowsArgumentException(string[] leftAttrbs, string[] rightAttrbs)
    {
      var left = new HashSet<string>(leftAttrbs);
      var right = new HashSet<string>(rightAttrbs);
      var ex = Assert.Throws<ArgumentException>(() => new FunctionalDependency(left, right));
      Assert.Equal(FunctionalDependency.NonNullAndNonEmptyAttributesMessage, ex.Message);
    }

    [Theory]
    [InlineData("a", "c", "a", "c")]
    [InlineData("a,b", "d,e,f", "b,a", "e,f,d")]
    [InlineData("a", "d,c", "a", "c,d")]
    public void DoubleEquals_DifferentFdObjs_ReturnsTrue(string left1, string right1, string left2, string right2)
    {
      var fd1 = FuncDepUtils.ConstructFdFromString(left1, right1);
      var fd2 = FuncDepUtils.ConstructFdFromString(left2, right2);
      Assert.True(fd1 == fd2);
    }

    [Theory]
    [InlineData("a", "c", "a", "c")]
    [InlineData("a,b", "d,e,f", "b,a", "e,f,d")]
    [InlineData("a", "d,c", "a", "c,d")]
    public void NotEqual_DifferentFdObjs_ReturnsFalse(string left1, string right1, string left2, string right2)
    {
      var fd1 = FuncDepUtils.ConstructFdFromString(left1, right1);
      var fd2 = FuncDepUtils.ConstructFdFromString(left2, right2);
      Assert.False(fd1 != fd2);
    }

    [Theory]
    [InlineData("a", "c")]
    [InlineData("a,b", "d,e,f")]
    [InlineData("a", "d,c")]
    public void DoubleEquals_SameFdObj_ReturnsTrue(string left, string right)
    {
      var fd = FuncDepUtils.ConstructFdFromString(left, right);
      var tempFd = fd;
      Assert.True(fd == tempFd);
    }

    [Theory]
    [InlineData("a", "c")]
    [InlineData("a,b", "d,e,f")]
    [InlineData("a", "d,c")]
    public void NotEqual_SameFdObj_ReturnsFalse(string left, string right)
    {
      var fd = FuncDepUtils.ConstructFdFromString(left, right);
      var tempFd = fd;
      Assert.False(fd != tempFd);
    }

    [Theory]
    [InlineData("a", "c", "a", "d")]
    [InlineData("a", "c", "b", "c")]
    [InlineData("a", "c", "b,e,f", "c")]
    [InlineData("a", "c", "b", "c,e,f")]
    [InlineData("a,b,c", "e,d", "a,x,y", "c,d")]
    public void DoubleEquals_DifferentFdObjs_ReturnsFalse(string left1, string right1, string left2, string right2)
    {
      var fd1 = FuncDepUtils.ConstructFdFromString(left1, right1);
      var fd2 = FuncDepUtils.ConstructFdFromString(left2, right2);
      Assert.False(fd1 == fd2);
    }

    [Theory]
    [InlineData("a", "c", "a", "d")]
    [InlineData("a", "c", "b", "c")]
    [InlineData("a", "c", "b,e,f", "c")]
    [InlineData("a", "c", "b", "c,e,f")]
    [InlineData("a,b,c", "e,d", "a,x,y", "c,d")]
    public void NotEqual_DifferentFdObjs_ReturnsTrue(string left1, string right1, string left2, string right2)
    {
      var fd1 = FuncDepUtils.ConstructFdFromString(left1, right1);
      var fd2 = FuncDepUtils.ConstructFdFromString(left2, right2);
      Assert.True(fd1 != fd2);
    }

    [Fact]
    public void DoubleEquals_OneNullObj_ReturnsFalse()
    {
      var fd = FuncDepUtils.ConstructFdFromString("a", "b");
      Assert.False(fd == null);
      Assert.False(null == fd);
    }

    [Fact]
    public void NotEqual_OneNullObj_ReturnsTrue()
    {
      var fd = FuncDepUtils.ConstructFdFromString("a", "b");
      Assert.True(fd != null);
      Assert.True(null != fd);
    }

    [Fact]
    public void DoubleEquals_TwoNullObjs_ReturnsTrue()
    {
      FunctionalDependency? fd = null;
      Assert.True(fd == null);
      Assert.True(null == fd);
    }

    [Fact]
    public void NotEqual_TwoNullObjs_ReturnsFalse()
    {
      FunctionalDependency? fd = null;
      Assert.False(fd != null);
      Assert.False(null != fd);
    }

    [Theory]
    [InlineData("a", "c", 0)]
    [InlineData("a", "c", 0.0f)]
    [InlineData("a", "c", 'x')]
    [InlineData("a", "c", "hello")]
    [InlineData("a", "c", null)]
    public void Equals_OtherObj_ReturnsFalse(string left, string right, object obj)
    {
      var fd = FuncDepUtils.ConstructFdFromString(left, right);
      Assert.False(fd.Equals(obj));
    }

    [Fact]
    public void Equals_NullArgument_ReturnsFalse()
    {
      var fd = FuncDepUtils.ConstructFdFromString("A,B", "C", ",");
      Assert.False(fd.Equals(null));
    }

    [Theory]
    [InlineData("a", "c", "a", "c")]
    [InlineData("a,b", "d,e,f", "b,a", "e,f,d")]
    [InlineData("a", "d,c", "a", "c,d")]
    [InlineData("a,b,c,d,e,f,g", "x", "a,b,c,d,e,f,g", "x")]
    [InlineData("a,b,c,d,e,f,g", "x,y,z", "b,a,c,d,e,f,g", "x,y,z")]
    public void GetHashCode_CompareTwoHashCodes_ReturnsTrue(string left1, string right1, string left2, string right2)
    {
      var fd1 = FuncDepUtils.ConstructFdFromString(left1, right1);
      var fd2 = FuncDepUtils.ConstructFdFromString(left2, right2);
      var hc1 = fd1.GetHashCode();
      var hc2 = fd2.GetHashCode();
      Assert.True(hc1 == hc2, $"Hash codes don't match. {hc1} != {hc2}");
    }
  
  }
}
