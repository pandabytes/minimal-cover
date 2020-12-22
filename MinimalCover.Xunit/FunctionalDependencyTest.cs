using System;
using System.Collections.Generic;
using MinimalCover.Core;
using Xunit;

namespace MinimalCover.Xunit
{
  public class FunctionalDependencyTest
  {
    [Theory]
    [InlineData("a", "c", "a", "c")]
    [InlineData("a,b", "d,e,f", "b,a", "e,f,d")]
    [InlineData("a", "d,c", "a", "c,d")]
    public void Equals_Test(string left1, string right1, string left2, string right2)
    {
      var fd1 = new FunctionalDependency(left1, right1);
      var fd2 = new FunctionalDependency(left2, right2);
      var tempRef = fd1;

      Assert.True(fd1 == fd2);
      Assert.True(fd1.Equals(fd2));
      Assert.True(fd1.Equals(tempRef));
    }

    [Theory]
    [InlineData("a", "c", "a", "d")]
    [InlineData("a", "c", "b", "c")]
    [InlineData("a,b,c", "e,d", "a,x,y", "c,d")]
    public void Not_Equals_Test(string left1, string right1, string left2, string right2)
    {
      var fd1 = new FunctionalDependency(left1, right1);
      var fd2 = new FunctionalDependency(left2, right2);

      Assert.True(fd1 != fd2);
      Assert.False(fd1 == null);
      Assert.False(fd1.Equals(fd2));
      Assert.False(fd1.Equals(null));  
    }

    [Theory]
    [InlineData("a", "c", "a", "c")]
    [InlineData("a,b", "d,e,f", "b,a", "e,f,d")]
    [InlineData("a", "d,c", "a", "c,d")]
    [InlineData("a,b,c,d,e,f,g", "x", "a,b,c,d,e,f,g", "x")]
    [InlineData("a,b,c,d,e,f,g", "x,y,z", "a,b,c,d,e,f,g", "x,y,z")]
    public void Hashcode_Test(string left1, string right1, string left2, string right2)
    {
      var fd1 = new FunctionalDependency(left1, right1);
      var fd2 = new FunctionalDependency(left2, right2);
      Assert.True(fd1.GetHashCode() == fd2.GetHashCode(), "Hash codes don't match");
    }
  
    [Theory]
    [InlineData(true, "a", "a,b", "d,e,f")]
    [InlineData(true, "b", "a,b", "d,e,f")]
    [InlineData(true, "c", "a,b,c,d,e,f,g", "x,y,z")]
    [InlineData(false, "x", "a,b", "d,e,f")]
    [InlineData(false, "y", "a,b", "d,e,f")]
    [InlineData(false, "x", "a,b,c,d,e,f,g", "x,y,z")]
    public void IsOnLeft_Single_Attribute_Test(bool expected, string attrb, string left, string right)
    {
      var fd = new FunctionalDependency(left, right);
      Assert.True(expected == fd.IsOnLeft(attrb));
    }

    [Theory]
    [InlineData(true, new string[] { "a" }, "a,b", "d,e,f")]
    [InlineData(true, new string[] { "a", "b" }, "a,b", "d,e,f")]
    [InlineData(true, new string[] { "a", "f", "g" }, "a,b,c,d,e,f,g", "x,y,z")]
    [InlineData(false, new string[] { "x" }, "a,b", "d,e,f")]
    [InlineData(false, new string[] { "a", "f", "g", "x" }, "a,b,c,d,e,f,g", "x,y,z")]
    public void IsOnLeft_Many_Attributes_Test(bool expected, string[] attrbs, string left, string right)
    {
      var fd = new FunctionalDependency(left, right);
      Assert.True(expected == fd.IsOnLeft(attrbs));
    }

    [Theory]
    [InlineData(true, "d", "a,b", "d,e,f")]
    [InlineData(true, "e", "a,b", "d,e,f")]
    [InlineData(true, "x", "a,b,c,d,e,f,g", "x,y,z")]
    [InlineData(false, "a", "a,b", "d,e,f")]
    [InlineData(false, "b", "a,b", "d,e,f")]
    [InlineData(false, "c", "a,b,c,d,e,f,g", "x,y,z")]
    public void IsOnRight_Single_Attribute_Test(bool expected, string attrb, string left, string right)
    {
      var fd = new FunctionalDependency(left, right);
      Assert.True(expected == fd.IsOnRight(attrb));
    }

    [Theory]
    [InlineData(true, new string[] { "f" }, "a,b", "d,e,f")]
    [InlineData(true, new string[] { "d", "e" }, "a,b", "d,e,f")]
    [InlineData(true, new string[] { "x", "y", "z" }, "a,b,c,d,e,f,g", "x,y,z")]
    [InlineData(false, new string[] { "a" }, "a,b", "d,e,f")]
    [InlineData(false, new string[] { "b", "f", "g", "x" }, "a,b,c,d,e,f,g", "x,y,z")]
    public void IsOnRight_Many_Attributes_Test(bool expected, string[] attrbs, string left, string right)
    {
      var fd = new FunctionalDependency(left, right);
      Assert.True(expected == fd.IsOnRight(attrbs));
    }
  }
}
