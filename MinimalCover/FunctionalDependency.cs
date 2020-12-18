using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace MinimalCover
{
  public class FunctionalDependency
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

    //public bool Equals(FunctionalDependency otherFd)
    //{
    //  return otherFd != null && Left == otherFd.Left && Right == otherFd.Right;
    //}
    public override bool Equals(object obj)
    {
      var otherFd = obj as FunctionalDependency;
      if (otherFd != null)
      {
        return Left.SetEquals(otherFd.Left) && Right.SetEquals(otherFd.Right);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
