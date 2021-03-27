using System;
using System.Collections.Generic;
using MinimalCover.Domain.Models;

namespace MinimalCover.Application.Algorithms
{
  /// <summary>
  /// Describe a set of methods that are required to find 
  /// the minimum cover, given a set of <see cref="FunctionalDependency"/>
  /// </summary>
  public interface IMinimalCover
  {
    /// <summary>
    /// Get all functional dependencies that have only have 1 attribute on RHS.
    /// If a functional dependency already has 1 attribute on RHS, then
    /// nothing is performed and it will be included in the returned set as well
    /// </summary>
    /// <param name="fds">Set of functional dependencies</param>
    /// <returns>Set of functional dependencies that have only 1 attribute on RHS</returns>
    ISet<FunctionalDependency> GetSingleRhsAttributeFds(ISet<FunctionalDependency> fds);

    /// <summary>
    /// Remove any extraneous attributes on LHS of each functional dependency
    /// </summary>
    /// <param name="fds">Set of functional dependencies</param>
    /// <returns>
    /// Set of functional dependencies that have extraneous attributes removed on LHS
    /// </returns>
    ISet<FunctionalDependency> RemoveExtrasLhsAttributes(ISet<FunctionalDependency> fds);

    /// <summary>
    /// Remove any extraneous functional dependency
    /// </summary>
    /// <param name="fds">Set of functional dependencies</param>
    /// <returns>
    /// Set of functional dependencies have no extranenous functional dependencies
    /// </returns>
    ISet<FunctionalDependency> RemoveExtraFds(ISet<FunctionalDependency> fds);
  }
}
