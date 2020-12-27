using System;
using System.Collections.Generic;
using MinimalCover.Core;
using Xunit;

namespace MinimalCover.Core.Xunit
{
  public class ReadOnlySetTest
  {
    [Fact]
    public void Not_Supported_Methods_Test()
    {
      var set = new HashSet<string>() { "a", "b", "c" };
      var readOnlySet = new ReadOnlySet<string>(set);
      
      Assert.Throws<NotSupportedException>(() => ((ISet<string>)readOnlySet).Add("a"));
      Assert.Throws<NotSupportedException>(() => ((ICollection<string>)readOnlySet).Add("a"));
      Assert.Throws<NotSupportedException>(() => readOnlySet.Clear());
      Assert.Throws<NotSupportedException>(() => readOnlySet.ExceptWith(null));
      Assert.Throws<NotSupportedException>(() => readOnlySet.IntersectWith(null));
      Assert.Throws<NotSupportedException>(() => readOnlySet.Remove("a"));
      Assert.Throws<NotSupportedException>(() => readOnlySet.SymmetricExceptWith(null));
      Assert.Throws<NotSupportedException>(() => readOnlySet.UnionWith(null));
    }

    [Theory]
    [InlineData(new object[] { 1,2,3 }, new object[] { 1,2,3 })]
    [InlineData(new object[] { "a", "b", "c" }, new object[] { "a", "b", "c" })]
    public void Equal_Test(object[] a, object[] b)
    {
      var readOnlySetA = new ReadOnlySet<object>(new HashSet<object>(a));
      var readOnlySetB = new ReadOnlySet<object>(new HashSet<object>(b));
      var tempRef = readOnlySetA;

      // Test ==
      Assert.True(readOnlySetA == readOnlySetB, "Sets are not equal via ==");
      Assert.True(readOnlySetA == tempRef, "Set is not equal to itself via ==");
      Assert.False(readOnlySetA == null, "Set is not supposed to be equal to null via ==");

      // Test Equals
      Assert.True(readOnlySetA.Equals(readOnlySetB), "Sets are not equal via Equals");
      Assert.True(readOnlySetA.Equals(tempRef), "Set is not equal to itself via Equals");
      Assert.False(readOnlySetA.Equals(null), "Set is not supposed to be equal to null via Equals");
    }

    [Theory]
    [InlineData(new object[] { 1, 2, 3 }, new object[] { 1 })]
    [InlineData(new object[] { 1, 2, 3 }, new object[] { 1, 2 })]
    [InlineData(new object[] { 1, 2, 3 }, new object[] { 1, 2, 4 })]
    public void Not_Equal_Test(object[] a, object[] b)
    {
      var readOnlySetA = new ReadOnlySet<object>(new HashSet<object>(a));
      var readOnlySetB = new ReadOnlySet<object>(new HashSet<object>(b));

      Assert.True(readOnlySetA != readOnlySetB, "Sets are not supposed to be equal");
      Assert.True(readOnlySetA != null, "Set is supposed to be not equal to null");
    }


    [Theory]
    [InlineData(new object[] { 1 }, new object[] { 1 })]
    [InlineData(new object[] { 1, 2, 3 }, new object[] { 1, 2, 3 })]
    [InlineData(new object[] { "a", "b", "c" }, new object[] { "a", "b", "c" })]
    public void Hashcode_Test(object[] a, object[] b)
    {
      var readOnlySetA = new ReadOnlySet<object>(new HashSet<object>(a));
      var readOnlySetB = new ReadOnlySet<object>(new HashSet<object>(b));

      Assert.True(readOnlySetA.GetHashCode() == readOnlySetB.GetHashCode(), "Hash codes don't match");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(20)]
    [InlineData(5000)]
    public void Count_Test(int count)
    {
      var set = new HashSet<int>();
      for (int i = 0; i < count; i++)
      {
        set.Add(i);
      }
      
      var readOnlySet = new ReadOnlySet<int>(set);
      Assert.True(readOnlySet.Count == count, $"ReadOnlySet doesn't have {count} items");
    }

  }
}
