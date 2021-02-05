using System;
using System.Collections.Generic;
using MinimalCover.Domain.Models;

namespace MinimalCover.Application.Algorithms
{
  public interface IMinimalCover
  {
    AttributeSet ComputeClosure(ISet<string> attributes, ISet<FunctionalDependency> fds);

    AttributeSet ComputeClosure(string attribute, ISet<FunctionalDependency> fds);

    ISet<FunctionalDependency> GetSingleRhsAttributeFds(ISet<FunctionalDependency> fds);

    ISet<FunctionalDependency> RemoveExtrasLhsAttributes(ISet<FunctionalDependency> fds);

    ISet<FunctionalDependency> RemoveExtraFds(ISet<FunctionalDependency> fds);
  }
}
