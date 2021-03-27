using System;
using System.Collections.Generic;

namespace MinimalCover.Domain.Models
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

    /// <summary>
    /// Construct a functional dependency object
    /// </summary>
    /// <param name="left">Left attributes</param>
    /// <param name="right">Right attributes</param>
    public FunctionalDependency(ISet<string> left, ISet<string> right)
    {
      Left = new AttributeSet(left);
      Right = new AttributeSet(right);
    }

    public override string ToString() => $"{Left} --> {Right}";

    public static bool operator ==(FunctionalDependency? a, FunctionalDependency? b)
    {
      if (a is null && b is null)
      {
        return true;
      }

      if (a is not null)
      {
        return a.Equals(b);
      }
      return b!.Equals(a);
    }

    public static bool operator !=(FunctionalDependency? a, FunctionalDependency? b) => !(a == b);

    public override bool Equals(object? obj)
    {
      if (ReferenceEquals(this, obj))
      {
        return true;
      }

      if (obj is FunctionalDependency)
      {
        var otherFd = (FunctionalDependency)obj;
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
