using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MinimalCover
{
  public class Program
  {
    public static IEnumerable<FunctionalDependency> SingleAttributeRhs(FunctionalDependency fd)
    {
      var fds = new List<FunctionalDependency>();
      foreach (var attribute in fd.Right)
      {
        var newFd = new FunctionalDependency(fd.Left, attribute);
        fds.Add(newFd);
      }
      return fds;
    }

    public static AttributeSet ComputeClosure(string attribute, IEnumerable<FunctionalDependency> fds)
    {
      foreach (var fd in fds)
      {
        if (fd.Right.Count > 1)
        {
          throw new ArgumentException("Every functional dependency must only have 1 attribute on the \"right\"");
        }
      }

      var closure = new HashSet<string>() { attribute };
      var iterateStack = new Stack<FunctionalDependency>(fds);
      var discardStack = new Stack<FunctionalDependency>(iterateStack.Count);

      bool closureUpdate;
      do
      {
        closureUpdate = false;
        while (iterateStack.Count > 0)
        {
          var fd = iterateStack.Pop();
          if (fd.Left.IsSubsetOf(closure))
          {
            // There should only be 1 attribute on the right
            closure.Add(fd.Right.First());
            closureUpdate = true;
          }
          else
          {
            // Save the fd in case if the closure is updated,
            // then we can look at this fd again and it may 
            // be a subset of the updated closure
            discardStack.Push(fd);
          }
        }

        if (closureUpdate)
        {
          // Refill the iterate stack
          while (discardStack.Count > 0)
          {
            iterateStack.Push(discardStack.Pop());
          }
        }
      } while (closureUpdate); // No update - stop

      return new AttributeSet(closure);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
      var givenFds = new FunctionalDependency[5] {
        new FunctionalDependency("A", "D"),
        new FunctionalDependency("B,C", "A,D"),
        new FunctionalDependency("C", "B"),
        new FunctionalDependency("E", "A"),
        new FunctionalDependency("E", "D")
      };
      var relation = new Relation(givenFds);

      // 1. Single attribute RHS
      var fdsList = new List<FunctionalDependency>();
      foreach (var fd in givenFds)
      {
        if (fd.Right.Count > 1)
        {
          var newFds = SingleAttributeRhs(fd);
          fdsList.AddRange(newFds);
        }
        else
        {
          fdsList.Add(fd);
        }
      }

      // 2. Remove extranenous attributes on LHS
      var manyAttributesLhs = fdsList.Where((fd) => fd.Left.Count > 1);
      for (int i = fdsList.Count - 1; i >= 0; i--)
        //foreach (var fd in manyAttributesLhs)
      {
        var fd = fdsList[i];
        if (fd.Left.Count > 1) 
        {
          var attributeSet = new HashSet<string>(fd.Left);
          foreach (var attribute in fd.Left)
          {
            // Compute the closure of the current attribute
            // Remove any attribute that exist in the intersection
            // of the closure and fd.Left
            var closure = ComputeClosure(attribute, fdsList);
            var intersection = new HashSet<string>(closure.Intersect(attributeSet));
            foreach (var intersectAttribute in intersection)
            {
              if (intersectAttribute != attribute && attributeSet.Contains(intersectAttribute))
              {
                attributeSet.Remove(intersectAttribute);
              }
            }
          }

          // If true, it means extraneous attributes have been removed
          if (!attributeSet.SetEquals(fd.Left))
          {
            fdsList.Remove(fd);
            fdsList.Add(new FunctionalDependency(attributeSet, fd.Right));
          }
        }
      }

      foreach (var fd in fdsList)
      {
        Console.WriteLine(fd);
      }
      
    }
  }
}
