using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using MinimalCover.Domain.Core;

namespace MinimalCover.Domain.UnitTests.Core
{
  /// <summary>
  /// Inhereit <see cref="ReadOnlySet{T}"/> to use some of
  /// its protected members, such as some constants
  /// </summary>
  public class ReadOnlySetTests : ReadOnlySet<object>
  {
    /// <summary>
    /// Constructor that creates an empty readonly set to 
    /// satisfy the C# inhereitance from <see cref="ReadOnlySet{T}"/>
    ///
    /// Nothing is done in this constructor.
    /// </summary>
    public ReadOnlySetTests()
      : base(new HashSet<object>())
    { }

    [Fact]
    public void NotSupportedMethods_ThrowsNotSupportedException()
    {
      var set = new HashSet<string>() { "a", "b", "c" };
      var readOnlySet = new ReadOnlySet<string>(set);
      var emptyEnumerable = Enumerable.Empty<string>();

      var ex = Assert.Throws<NotSupportedException>(() => ((ISet<string>)readOnlySet).Add("a"));
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => ((ICollection<string>)readOnlySet).Add("a"));
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => readOnlySet.Clear());
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => readOnlySet.ExceptWith(emptyEnumerable));
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => readOnlySet.IntersectWith(emptyEnumerable));
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => readOnlySet.Remove("a"));
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => readOnlySet.SymmetricExceptWith(emptyEnumerable));
      Assert.Equal(ReadonlySetMessage, ex.Message);

      ex = Assert.Throws<NotSupportedException>(() => readOnlySet.UnionWith(emptyEnumerable));
      Assert.Equal(ReadonlySetMessage, ex.Message);
    }

    [Fact]
    public void DoubleEquals_OneNullObj_ReturnsFalse()
    {
      var set = new HashSet<string>() { "a", "b" };
      var readonlySet = new ReadOnlySet<string>(set);

      Assert.False(readonlySet == null);
      Assert.False(null == readonlySet);
    }

    [Fact]
    public void NotEqual_OneNullObj_ReturnsTrue()
    {
      var set = new HashSet<string>() { "a", "b" };
      var readonlySet = new ReadOnlySet<string>(set);

      Assert.True(readonlySet != null);
      Assert.True(null != readonlySet);
    }

    [Theory]
    [InlineData(new object[] { 1, 2, 3 })]
    [InlineData(new object[] { 'a', 'b', 'c' })]
    public void DoubleEquals_SameObj_ReturnsTrue(params object[] items)
    {
      var readOnlySet = new ReadOnlySet<object>(new HashSet<object>(items));
      var tempReadonlySet = readOnlySet;
      Assert.True(readOnlySet == tempReadonlySet);
    }

    [Theory]
    [InlineData(new object[] { 1, 2, 3 })]
    [InlineData(new object[] { 'a', 'b', 'c' })]
    public void NotEqual_SameObj_ReturnsFalse(params object[] items)
    {
      var readOnlySet = new ReadOnlySet<object>(new HashSet<object>(items));
      var tempReadonlySet = readOnlySet;
      Assert.False(readOnlySet != tempReadonlySet);
    }

    [Theory]
    [InlineData(new object[] { 1, 2, 3 }, new object[] { 1, 2, 3 })]
    [InlineData(new object[] { "a", "b", "c" }, new object[] { "a", "b", "c" })]
    public void DoubleEquals_DifferentReadOnlySetObjs_ReturnsTrue(object[] a, object[] b)
    {
      var readOnlySetA = new ReadOnlySet<object>(new HashSet<object>(a));
      var readOnlySetB = new ReadOnlySet<object>(new HashSet<object>(b));
      Assert.True(readOnlySetA == readOnlySetB);
    }

    [Theory]
    [InlineData(new object[] { 1, 2, 3 }, new object[] { 1, 2, 3 })]
    [InlineData(new object[] { "a", "b", "c" }, new object[] { "a", "b", "c" })]
    public void NotEqual_DifferentReadOnlySetObjs_ReturnsFalse(object[] a, object[] b)
    {
      var readOnlySetA = new ReadOnlySet<object>(new HashSet<object>(a));
      var readOnlySetB = new ReadOnlySet<object>(new HashSet<object>(b));
      Assert.False(readOnlySetA != readOnlySetB);
    }

    [Theory]
    [InlineData(new object[] { 1, 2, 4 }, new object[] { 1, 2, 3 })]
    [InlineData(new object[] { "a", "bx", "c" }, new object[] { "a", "b", "c" })]
    public void DoubleEquals_DifferentReadOnlySetObjs_ReturnsFalse(object[] a, object[] b)
    {
      var readOnlySetA = new ReadOnlySet<object>(new HashSet<object>(a));
      var readOnlySetB = new ReadOnlySet<object>(new HashSet<object>(b));
      Assert.False(readOnlySetA == readOnlySetB);
    }

    [Theory]
    [InlineData(new object[] { 1, 2, 4 }, new object[] { 1, 2, 3 })]
    [InlineData(new object[] { "a", "bx", "c" }, new object[] { "a", "b", "c" })]
    public void NotEqual_DifferentReadOnlySetObjs_ReturnsTrue(object[] a, object[] b)
    {
      var readOnlySetA = new ReadOnlySet<object>(new HashSet<object>(a));
      var readOnlySetB = new ReadOnlySet<object>(new HashSet<object>(b));
      Assert.True(readOnlySetA != readOnlySetB);
    }

    [Fact]
    public void DoubleEquals_TwoNullObjs_ReturnsTrue()
    {
      ReadOnlySet<object>? readonlySet = null;
      Assert.True(readonlySet == null);
      Assert.True(null == readonlySet);
    }

    [Fact]
    public void NotEqual_TwoNullObjs_ReturnsFalse()
    {
      ReadOnlySet<object>? readonlySet = null;
      Assert.False(readonlySet != null);
      Assert.False(null != readonlySet);
    }

    [Fact]
    public void Equals_NullArgument_ReturnsFalse()
    {
      var readonlySet = new ReadOnlySet<int>(new HashSet<int>());
      Assert.False(readonlySet.Equals(null));
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
