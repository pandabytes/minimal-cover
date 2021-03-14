using System;
using System.Collections.Generic;
using System.Linq;
using MinimalCover.Application.Algorithms;
using MinimalCover.Domain.Models;

namespace MinimalCover.Infrastructure.Algorithms
{
  /// <summary>
  /// The default implementation of steps to
  /// find the minimal cover
  /// </summary>
  internal class DefaultMinimalCover : IMinimalCover
  {
    ISet<FunctionalDependency> IMinimalCover.GetSingleRhsAttributeFds(ISet<FunctionalDependency> fds)
    {
      var fdsSet = new HashSet<FunctionalDependency>();
      foreach (var fd in fds)
      {
        if (fd.Right.Count > 1)
        {
          var newFds = SingleRhsAttribute(fd);
          fdsSet.UnionWith(newFds);
        }
        else
        {
          fdsSet.Add(fd);
        }
      }
      return fdsSet;
    }

    ISet<FunctionalDependency> IMinimalCover.RemoveExtrasLhsAttributes(ISet<FunctionalDependency> fds)
    {
      var fdsSet = new HashSet<FunctionalDependency>(fds);
      var manyLhsAttributes = fds.Where(fd => fd.Left.Count > 1);

      foreach (var fd in manyLhsAttributes)
      {
        var nonExtraAttributes = new HashSet<string>(fd.Left);
        foreach (var attribute in fd.Left)
        {
          // Compute the closure of the current attribute
          // Remove any attribute that exist in the intersection
          // of the closure and fd.Left
          var closure = ComputeClosure(attribute, fds);
          var intersection = closure.Intersect(nonExtraAttributes).ToArray();

          nonExtraAttributes.RemoveWhere(a =>
          {
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

    /// <exception cref="ArgumentException">
    /// Raise this exception when at least 1 <see cref="FunctionalDependency"/>
    /// has more than 1 attribute on RHS
    /// </exception>
    ISet<FunctionalDependency> IMinimalCover.RemoveExtraFds(ISet<FunctionalDependency> fds)
    {
      CheckSingleAttributeRhs(fds);

      // Use list so we can iterate in reverse to make modification
      var fdsList = new List<FunctionalDependency>(fds);

      for (int i = fdsList.Count - 1; i >= 0; i--)
      {
        var fd = fdsList[i];

        // Compute closure of fd.Left without "looking" at fd
        var otherFds = fdsList.Where(otherFd => fd != otherFd).ToHashSet();
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
    /// Transform a functional dependency that has n attributes on RHS
    /// into n functional dependencies where each functional dependency
    /// has only 1 attribute on RHS
    /// </summary>
    /// <param name="fd">Functional dependency</param>
    /// <returns>Set of functional dependencies that only has 1 attribute on RHS</returns>
    private static ISet<FunctionalDependency> SingleRhsAttribute(FunctionalDependency fd)
    {
      var fds = new HashSet<FunctionalDependency>();
      foreach (var attribute in fd.Right)
      {
        var rightAttributeSet = new HashSet<string> { attribute };
        var newFd = new FunctionalDependency(fd.Left, rightAttributeSet);
        fds.Add(newFd);
      }
      return fds;
    }

    /// <summary>
    /// Check if all <see cref="fds"/> have a single attribute on RHS.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Raise this exception when at least 1 <see cref="FunctionalDependency"/>
    /// has more than 1 attribute on RHS
    /// </exception>
    /// <param name="fds">Collection of functional dependencies</param>
    private static void CheckSingleAttributeRhs(IEnumerable<FunctionalDependency> fds)
    {
      if (fds.Any(fd => fd.Right.Count > 1))
      {
        const string message = "Every functional dependency must only have 1 attribute on the \"right\"";
        throw new ArgumentException(message);
      }
    }

    /// <summary>
    /// Compute the closure of <paramref name="attributes"/> given <paramref name="fds"/>
    /// </summary>
    /// <param name="attributes">Set of attributes</param>
    /// <param name="fds">Set of functional dependencies</param>
    /// <returns>Set of attributes that form the closure</returns>
    private static AttributeSet ComputeClosure(ISet<string> attributes, ISet<FunctionalDependency> fds)
    {
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
            // Since fd.Left is a subset of closure then
            // it means the current closure can determine
            // fd.Right. So we union closure with fd.Right
            closure.UnionWith(fd.Right);
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
    /// Compute the closure of <paramref name="attribute"/> given <paramref name="fds"/>
    /// </summary>
    /// <param name="attribute">Attribute</param>
    /// <param name="fds">Set of functional dependencies</param>
    /// <returns>Set of attributes that form the closure</returns>
    private static AttributeSet ComputeClosure(string attribute, ISet<FunctionalDependency> fds)
    {
      return ComputeClosure(new HashSet<string> { attribute }, fds);
    }

  }
}
