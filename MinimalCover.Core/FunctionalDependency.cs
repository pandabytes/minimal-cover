using System;
using System.Collections.Generic;
using System.Linq;

namespace MinimalCover.Core
{
  /// <summary>
  /// Represent the functional dependency constraint in database
  /// where Left means the determinant set and Right means the
  /// set that functionally dependent on Left. In other words,
  /// Left ---> Right
  /// </summary>
  public sealed class FunctionalDependency
  {
    /// <summary>
    /// Determinant set of a functional dependency
    /// </summary>
    public AttributeSet Left { get; }

    /// <summary>
    /// Set that functionally dependent on <see cref="Left"/>
    /// </summary>
    public AttributeSet Right { get; }

    public FunctionalDependency(ISet<string> left, ISet<string> right)
    {
      Left = new AttributeSet(left);
      Right = new AttributeSet(right);
    }

    /// <summary>
    /// Construct a functional depedency by providing strings of
    /// attributes. Attributes are splited by <paramref name="sep"/>
    /// </summary>
    /// <param name="left">The string containing attributes on LHS</param>
    /// <param name="right">The string containing attributes on RHS</param>
    /// <param name="sep">
    /// The separator to split both <paramref name="left"/> and <paramref name="right"/>
    /// </param>
    public FunctionalDependency(string left, string right, char sep = ',')
    {
      Left = new AttributeSet(left, sep);
      Right = new AttributeSet(right, sep);
    }

    /// <summary>
    /// Construct a functional depedency by providing a string and
    /// a set of attributes. Attributes are splited by <paramref name="sep"/>
    /// </summary>
    /// <param name="left">The string containing attributes on LHS</param>
    /// <param name="right">Set containing attributes on RHS</param>
    /// <param name="sep">The separator to split only <paramref name="left"/></param>
    public FunctionalDependency(string left, ISet<string> right, char separator = ',')
    {
      Left = new AttributeSet(left, separator);
      Right = new AttributeSet(right);
    }

    /// <summary>
    /// Construct a functional depedency by providing a string and
    /// a set of attributes. Attributes are splited by <paramref name="sep"/>
    /// </summary>
    /// <param name="left">Set containing attributes on RHS</param>
    /// <param name="right">The string containing attributes on LHS</param>
    /// <param name="sep">The separator to split only <paramref name="right"/></param>
    public FunctionalDependency(ISet<string> left, string right, char separator = ',')
    {
      Left = new AttributeSet(left);
      Right = new AttributeSet(right, separator);
    }

    public bool IsOnLeft(string attribute) => Left.Contains(attribute);

    public bool IsOnLeft(IEnumerable<string> attributes) => attributes.All(a => IsOnLeft(a));

    public bool IsOnRight(string attribute) => Right.Contains(attribute);

    public bool IsOnRight(IEnumerable<string> attributes) => attributes.All(a => IsOnRight(a));

    public override string ToString() => $"{Left} --> {Right}";

    public static bool operator ==(FunctionalDependency a, FunctionalDependency b)
    {
      if ((object)a == null || (object)b == null)
      {
        return false;
      }
      return a.Equals(b);
    }

    public static bool operator !=(FunctionalDependency a, FunctionalDependency b) => !(a == b);

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(this, obj))
      {
        return true;
      }

      if (obj is FunctionalDependency)
      {
        var otherFd = obj as FunctionalDependency;
        return Left == otherFd.Left && Right == otherFd.Right;
      }

      return false;
    }

    public override int GetHashCode()
    {
      unchecked
      {
        int hashcode = 1430287;
        foreach (var item in Left)
        {
          hashcode *= item.GetHashCode();
        }
        foreach (var item in Right)
        {
          hashcode *= item.GetHashCode();
        }
        return hashcode * 17;
      }
    }
  }
}
