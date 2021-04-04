using System;
using System.Linq;
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
    public static readonly string NonEmptyLeftRightMessage = "Both left and right must have at least 1 attribute";
    public static readonly string NonNullAndNonEmptyAttributesMessage = "Both left and right must have non-null and non-empty attributes";

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
    /// <exception cref="ArgumentException">Thrown when
    /// <paramref name="left"/> and/or <paramref name="right"/>
    ///  is/are empty or an attribute in a set is null or empty
    /// </exception>
    /// <param name="left">Left attributes</param>
    /// <param name="right">Right attributes</param>
    public FunctionalDependency(ISet<string> left, ISet<string> right)
    {
      ValidateConstructorArgs(left, right);
      
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

    /// <summary>
    /// Validate the arguments in the constructor
    /// </summary>
    /// <remarks>
    /// This method checks whether Left and Right are different non-empty sets,
    /// where each attribute is not null or empty
    /// </remarks>
    /// <exception cref="ArgumentException">Thrown when
    /// <paramref name="left"/> and/or <paramref name="right"/>
    ///  is/are empty or an attribute in a set is null or empty
    /// </exception>
    /// <param name="left">Left side of the functional dependency</param>
    /// <param name="right">Right side of the functional dependency</param>
    private void ValidateConstructorArgs(ISet<string> left, ISet<string> right)
    {
      if (left.Count > 0 && right.Count > 0)
      {
        var hasEmptyLeftAttrbs = left.Any(a => string.IsNullOrWhiteSpace(a));
        var hasEmptyRightAttrbs = right.Any(a => string.IsNullOrWhiteSpace(a));
        if (hasEmptyLeftAttrbs || hasEmptyRightAttrbs)
        {
          throw new ArgumentException(NonNullAndNonEmptyAttributesMessage);
        }
      }
      else
      {
        throw new ArgumentException(NonEmptyLeftRightMessage);
      }
    }

  }
}
