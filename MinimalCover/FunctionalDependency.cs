using System;
using System.Collections.Generic;
using System.Linq;

namespace MinimalCover
{
  public sealed class FunctionalDependency
  {
    public AttributeSet Left { get; }

    public AttributeSet Right { get; }

    public FunctionalDependency(ISet<string> left, ISet<string> right)
    {
      Left = new AttributeSet(left);
      Right = new AttributeSet(right);
    }

    public FunctionalDependency(string left, string right, char sep = ',')
    {
      Left = new AttributeSet(left, sep);
      Right = new AttributeSet(right, sep);
    }

    public FunctionalDependency(string left, ISet<string> right, char separator = ',')
    {
      Left = new AttributeSet(left, separator);
      Right = new AttributeSet(right);
    }

    public FunctionalDependency(ISet<string> left, string right, char separator = ',')
    {
      Left = new AttributeSet(left);
      Right = new AttributeSet(right, separator);
    }

    public bool IsOnLeft(string attribute) => Left.Contains(attribute);

    public bool IsOnLeft(IEnumerable<string> attributes) => attributes.All(a => IsOnLeft(a));

    public bool IsOnRight(string attribute) => Right.Contains(attribute);

    public bool IsOnRight(IEnumerable<string> attributes) => attributes.All(a => IsOnRight(a));

    public override string ToString() => $"{Left} ---> {Right}";

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
