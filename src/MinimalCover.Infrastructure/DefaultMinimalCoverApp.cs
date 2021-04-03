using System;
using System.Collections.Generic;
using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Algorithms;

namespace MinimalCover.Application
{
  /// <summary>
  /// Default implementation of <see cref="MinimalCoverApp"/>
  /// </summary>
  internal class DefaultMinimalCoverApp : MinimalCoverApp
  {
    /// <inheritdoc/>
    public DefaultMinimalCoverApp(IMinimalCover minimalCover)
      : base(minimalCover)
    {}

    /// <inheritdoc/>
    public override ISet<FunctionalDependency> FindMinimalCover(IParser parser, string value)
    {
      var fds = parser.Parse(value);
      return FindMinimalCover(fds);
    }

    /// <inheritdoc/>
    public override ISet<FunctionalDependency> FindMinimalCover(ISet<FunctionalDependency> fds)
    {
      var singleRhsAttributeFds = m_minimalCover.GetSingleRhsAttributeFds(fds);
      var noExtraLhsAttributesFds = m_minimalCover.RemoveExtrasLhsAttributes(singleRhsAttributeFds);
      return m_minimalCover.RemoveExtraFds(noExtraLhsAttributesFds);
    }

  }
}
