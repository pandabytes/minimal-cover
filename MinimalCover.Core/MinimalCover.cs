using System;
using System.Collections.Generic;
using System.Linq;

namespace MinimalCover.Core
{
  /// <summary>
  /// Contain logic that computes the minimal cover.
  /// </summary>
  public static class MinimalCover
  {
    private static void CheckSingleAttributeRhs(IEnumerable<FunctionalDependency> fds)
    {
      if (fds.Any(fd => fd.Right.Count > 1))
      {
        throw new ArgumentException("Every functional dependency must only have 1 attribute on the \"right\"");
      }
    }

    public static ISet<FunctionalDependency> SingleAttributeRhs(FunctionalDependency fd)
    {
      var fds = new HashSet<FunctionalDependency>();
      foreach (var attribute in fd.Right)
      {
        var newFd = new FunctionalDependency(fd.Left, attribute);
        fds.Add(newFd);
      }
      return fds;
    }

    public static AttributeSet ComputeClosure(IEnumerable<string> attributes, IEnumerable<FunctionalDependency> fds)
    {
      CheckSingleAttributeRhs(fds);

      var closure = new HashSet<string>(attributes);
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

    public static AttributeSet ComputeClosure(string attribute, IEnumerable<FunctionalDependency> fds)
    {
      return ComputeClosure(new string[1] { attribute }, fds);
    }

    /// <summary>
    /// 1st step
    /// </summary>
    /// <param name="fds"></param>
    /// <returns></returns>
    public static ISet<FunctionalDependency> GetSingleAttributeRhsFds(IEnumerable<FunctionalDependency> fds)
    {
      var fdsSet = new HashSet<FunctionalDependency>();
      foreach (var fd in fds)
      {
        if (fd.Right.Count > 1)
        {
          var newFds = SingleAttributeRhs(fd);
          fdsSet.UnionWith(newFds);
        }
        else
        {
          fdsSet.Add(fd);
        }
      }
      return fdsSet;
    }

    /// <summary>
    /// 2nd step
    /// </summary>
    /// <param name="fds"></param>
    /// <returns></returns>
    public static ISet<FunctionalDependency> RemoveExtrasAttributesLhs(IEnumerable<FunctionalDependency> fds)
    {
      var fdsSet = new HashSet<FunctionalDependency>(fds);
      var manyAttributesLhs = fds.Where(fd => fd.Left.Count > 1);

      foreach (var fd in manyAttributesLhs)
      {
        var nonExtraAttributes = new HashSet<string>(fd.Left);
        foreach (var attribute in fd.Left)
        {
          // Compute the closure of the current attribute
          // Remove any attribute that exist in the intersection
          // of the closure and fd.Left
          var closure = ComputeClosure(attribute, fds);
          var intersection = closure.Intersect(nonExtraAttributes).ToArray();

          nonExtraAttributes.RemoveWhere(a => {
            return intersection.Contains(a) && a != attribute;
          });
        }

        // If true, it means extraneous attributes have been removed
        if (!nonExtraAttributes.SetEquals(fd.Left))
        {
          // Remove this fd and a new fd with the extraneous attributes removed
          fdsSet.Remove(fd);
          fdsSet.Add(new FunctionalDependency(nonExtraAttributes, fd.Right));
        }
      }
      return fdsSet;
    }

    /// <summary>
    /// 3rd step
    /// </summary>
    /// <param name="fds"></param>
    /// <returns></returns>
    public static ISet<FunctionalDependency> RemoveExtraFds(IEnumerable<FunctionalDependency> fds)
    {
      CheckSingleAttributeRhs(fds);

      // Use list so we can iterate in reverse to make modification
      var fdsList = new List<FunctionalDependency>(fds);

      for (int i = fdsList.Count - 1; i >= 0; i--)
      {
        var fd = fdsList[i];

        // Compute closure of fd.Left without "looking" at fd
        var otherFds = fdsList.Where(otherFd => fd != otherFd);
        var closure = ComputeClosure(fd.Left, otherFds);

        // There should only be 1 attribute on RHS.
        // If the closure contains the "right" attribute
        // then we don't need this fd
        if (closure.Contains(fd.Right.First()))
        {
          fdsList.RemoveAt(i);
        }
      }
      return new HashSet<FunctionalDependency>(fdsList);
    }

    /// <summary>
    /// Compute the minimal cover.
    /// </summary>
    /// <remarks>
    /// Call these methods in the following order:
    /// 1. <see cref="GetSingleAttributeRhsFds(IEnumerable{FunctionalDependency})"/>
    /// 2. <see cref="RemoveExtrasAttributesLhs(IEnumerable{FunctionalDependency})"/>
    /// 3. <see cref="RemoveExtraFds(IEnumerable{FunctionalDependency})"/>
    /// </remarks>
    /// <param name="fds">Collection of functional dependencies</param>
    /// <returns>The minimal cover set of functional dependencies</returns>
    public static ISet<FunctionalDependency> ComputeMinimalCover(IEnumerable<FunctionalDependency> fds)
    {
      // 1. Single attribute RHS
      var fdsSet = GetSingleAttributeRhsFds(fds);

      // 2. Remove extranenous attributes on LHS
      var noExtraLhs = RemoveExtrasAttributesLhs(fdsSet);

      // 3. Remove extra fds
      return RemoveExtraFds(noExtraLhs);
    }
  }
}
