using System;
using System.Collections.Generic;
using Xunit;

namespace MinimalCover.Core.Xunit
{
  /// <summary>
  /// Inhereit <see cref="ReadOnlySet{T}"/> to use some of
  /// its protected members, such as some constants
  /// </summary>
  public class ReadOnlySetTests : ReadOnlySet<object>
  {
    [Fact]
    public void NotSupportedMethods_ThrowsNotSupportedException()
    {
      var set = new HashSet<string>() { "a", "b", "c" };
      var readOnlySet = new ReadOnlySet<string>(set);

      var ex = Assert.Throws<NotSupportedException>(() => ((ISet<string>)readOnlySet).Add("a"));
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => ((ICollection<string>)readOnlySet).Add("a"));
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => readOnlySet.Clear());
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => readOnlySet.ExceptWith(null));
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => readOnlySet.IntersectWith(null));
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => readOnlySet.Remove("a"));
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => readOnlySet.SymmetricExceptWith(null));
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => readOnlySet.UnionWith(null));
      Assert.Equal(ReadonlySetMessage, ex.Message);
    }

    [Fact]
    public void DoubleEquals_OneNullObj_ReturnsFalse()
    {
      var set = new HashSet<string>() { "a", "b" };
      var readonlySet = new ReadOnlySet<string>(set);

      Assert.False(readonlySet == null, $"{readonlySet} is not supposed to be equal to null");
    }

    [Theory]
    [InlineData(new object[] { 1, 2, 3 })]
    [InlineData(new object[] { 'a', 'b', 'c' })]
    public void DoubleEquals_SameObj_ReturnsTrue(params object[] items)
    {
      var readOnlySet = new ReadOnlySet<object>(new HashSet<object>(items));
      var tempReadonlySet = readOnlySet;
      Assert.True(readOnlySet == tempReadonlySet, $"{readOnlySet} doesn't equal to itself");
    }

    [Theory]
    [InlineData(new object[] { 1, 2, 3 }, new object[] { 1, 2, 3 })]
    [InlineData(new object[] { "a", "b", "c" }, new object[] { "a", "b", "c" })]
    public void DoubleEquals_DifferentReadOnlySetObjs_ReturnsTrue(object[] a, object[] b)
    {
      var readOnlySetA = new ReadOnlySet<object>(new HashSet<object>(a));
      var readOnlySetB = new ReadOnlySet<object>(new HashSet<object>(b));
      Assert.True(readOnlySetA == readOnlySetB, $"{readOnlySetA} doesn't equal to {readOnlySetB}");
    }

    [Theory]
    [InlineData(new object[] { 1, 2, 4 }, new object[] { 1, 2, 3 })]
    [InlineData(new object[] { "a", "bx", "c" }, new object[] { "a", "b", "c" })]
    public void DoubleEquals_DifferentReadOnlySetObjs_ReturnsFalse(object[] a, object[] b)
    {
      var readOnlySetA = new ReadOnlySet<object>(new HashSet<object>(a));
      var readOnlySetB = new ReadOnlySet<object>(new HashSet<object>(b));
      Assert.False(readOnlySetA == readOnlySetB, $"{readOnlySetA} is not supposed to be equal to {readOnlySetB}");
    }

    [Theory]
    [InlineData('c')]
    [InlineData("hello")]
    [InlineData(0.0f)]
    [InlineData(0)]
    public void DoubleEquals_OtherObj_ReturnsFalse(object obj)
    {
      var readonlySet = new ReadOnlySet<int>(new HashSet<int>() { 0 });
#pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
      Assert.False(readonlySet == obj, $"");
#pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast
    }

    [Theory]
    [InlineData(new object[] { 1 }, new object[] { 1 })]
    [InlineData(new object[] { 1, 2, 3 }, new object[] { 1, 2, 3 })]
    [InlineData(new object[] { "a", "b", "c" }, new object[] { "a", "b", "c" })]
    public void GetHashCode_CompareTwoHashCodes_ReturnsTrue(object[] a, object[] b)
    {
      var readOnlySetA = new ReadOnlySet<object>(new HashSet<object>(a));
      var readOnlySetB = new ReadOnlySet<object>(new HashSet<object>(b));
      var hcA = readOnlySetA.GetHashCode();
      var hcB = readOnlySetA.GetHashCode();
      Assert.True(readOnlySetA.GetHashCode() == readOnlySetB.GetHashCode(), $"Hash codes don't match. {hcA} != {hcB}");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(20)]
    [InlineData(5000)]
    public void Count_SimpleAdd_ReturnsNCount(int count)
    {
      var set = new HashSet<int>();
      for (int i = 0; i < count; i++)
      {
        set.Add(i);
      }
      
      var readOnlySet = new ReadOnlySet<int>(set);
      Assert.Equal(count, readOnlySet.Count);
    }
    
  }
}
