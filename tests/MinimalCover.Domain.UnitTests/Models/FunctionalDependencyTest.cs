using Xunit;
using System;
using MinimalCover.UnitTests.Utils;

namespace MinimalCover.Domain.UnitTests.Models
{
  public class FunctionalDependencyTests
  {
    [Theory]
    [InlineData("a", "c", "a", "c")]
    [InlineData("a,b", "d,e,f", "b,a", "e,f,d")]
    [InlineData("a", "d,c", "a", "c,d")]
    public void DoubleEquals_DifferentFdObjs_ReturnsTrue(string left1, string right1, string left2, string right2)
    {
      var fd1 = FuncDepUtils.ConstructFdFromString(left1, right1);
      var fd2 = FuncDepUtils.ConstructFdFromString(left2, right2);
      Assert.True(fd1 == fd2, $"{fd1} doesn't equal to {fd2}");
    }

    [Theory]
    [InlineData("a", "c", "a", "c")]
    [InlineData("a,b", "d,e,f", "b,a", "e,f,d")]
    [InlineData("a", "d,c", "a", "c,d")]
    public void NotEqual_DifferentFdObjs_ReturnsFalse(string left1, string right1, string left2, string right2)
    {
      var fd1 = FuncDepUtils.ConstructFdFromString(left1, right1);
      var fd2 = FuncDepUtils.ConstructFdFromString(left2, right2);
      Assert.False(fd1 != fd2, $"{fd1} is incorrectly equal to {fd2}");
    }

    [Theory]
    [InlineData("a", "c")]
    [InlineData("a,b", "d,e,f")]
    [InlineData("a", "d,c")]
    public void DoubleEquals_SameFdObj_ReturnsTrue(string left, string right)
    {
      var fd = FuncDepUtils.ConstructFdFromString(left, right);
      var tempFd = fd;
      Assert.True(fd == tempFd, $"{fd} doesn't equal to itself");
    }

    [Theory]
    [InlineData("a", "c")]
    [InlineData("a,b", "d,e,f")]
    [InlineData("a", "d,c")]
    public void NotEqual_SameFdObj_ReturnsFalse(string left, string right)
    {
      var fd = FuncDepUtils.ConstructFdFromString(left, right);
      var tempFd = fd;
      Assert.False(fd != tempFd, $"{fd} is incorrectly equal to itself");
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
      Assert.False(fd1 == fd2, $"{fd1} is not supposed to be equal to {fd2}");
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
      Assert.True(fd1 != fd2, $"{fd1} is incorrectly equal to {fd2}");
    }

    [Fact]
    public void DoubleEquals_OneNullObj_ReturnsFalse()
    {
      var fd = FuncDepUtils.ConstructFdFromString("a", "b");
      Assert.False(fd == null, $"{fd} is not supposed to be equal to null");
      Assert.False(null == fd, $"{fd} is not supposed to be equal to null");
    }

    [Fact]
    public void NotEqual_OneNullObj_ReturnsTrue()
    {
      var fd = FuncDepUtils.ConstructFdFromString("a", "b");
      Assert.True(fd != null, $"{fd} is incorrectly equal to null");
      Assert.True(null != fd, $"{fd} is incorrectly equal to null");
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
      Assert.False(fd.Equals(obj), $"{fd} is not supposed to be equal to {obj}");
    }

    [Fact]
    public void Equals_NullArgument_ReturnsFalse()
    {
      var fd = FuncDepUtils.ConstructFdFromString("A,B", "C", ",");
      Assert.False(fd.Equals(null), $"Functional dependency is not supposed to be equal to null");
    }

  [Theory]
    [InlineData("a", "c", "a", "c")]
    [InlineData("a,b", "d,e,f", "b,a", "e,f,d")]
    [InlineData("a", "d,c", "a", "c,d")]
    [InlineData("a,b,c,d,e,f,g", "x", "a,b,c,d,e,f,g", "x")]
    [InlineData("a,b,c,d,e,f,g", "x,y,z", "a,b,c,d,e,f,g", "x,y,z")]
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
