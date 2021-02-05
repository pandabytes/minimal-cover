using System;
using System.Collections.Generic;
using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Algorithms;

namespace MinimalCover.Application
{
  /// <summary>
  /// The main application that finds the minimal cover.
  /// This is where the logic of the application is defined.
  /// </summary>
  public class MinimalCoverApp
  {
    private readonly IMinimalCover m_minimalCover;

    public MinimalCoverApp(IMinimalCover minimalCover)
    {
      _ = minimalCover ?? throw new ArgumentNullException(nameof(minimalCover));
      m_minimalCover = minimalCover;
    }

    public ISet<FunctionalDependency> FindMinimalCover(string value, IParser parser)
    {
      var fds = parser.Parse(value);
      return FindMinimalCover(fds);
    }

    public ISet<FunctionalDependency> FindMinimalCover(ISet<FunctionalDependency> fds)
    {
      var singleRhsAttributeFds = m_minimalCover.GetSingleRhsAttributeFds(fds);
      var noExtraLhsAttributesFds = m_minimalCover.RemoveExtrasLhsAttributes(singleRhsAttributeFds);
      return m_minimalCover.RemoveExtraFds(noExtraLhsAttributesFds);
    }

  }
}
