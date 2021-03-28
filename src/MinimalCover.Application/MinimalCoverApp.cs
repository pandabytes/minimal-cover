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

    /// <summary>
    /// Constructor
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="minimalCover"/> is null
    /// </exception>
    /// <param name="minimalCover">Minimal cover algorithm</param>
    public MinimalCoverApp(IMinimalCover minimalCover)
    {
      m_minimalCover = minimalCover;
    }

    /// <summary>
    /// Find the minimal cover
    /// </summary>
    /// <param name="value">String value to be parsed</param>
    /// <param name="parser">The parser</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> or <paramref name="parser"/> is null
    /// </exception>
    /// <returns>A set of <see cref="FunctionalDependency"/></returns>
    public ISet<FunctionalDependency> FindMinimalCover(string value, IParser parser)
    {
      var fds = parser.Parse(value);
      return FindMinimalCover(fds);
    }

    /// <summary>
    /// Find the minimal cover
    /// </summary>
    /// <param name="fds">Set of <see cref="FunctionalDependency"/></param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="fds"/> is null
    /// </exception>
    /// <returns></returns>
    public ISet<FunctionalDependency> FindMinimalCover(ISet<FunctionalDependency> fds)
    {
      var singleRhsAttributeFds = m_minimalCover.GetSingleRhsAttributeFds(fds);
      var noExtraLhsAttributesFds = m_minimalCover.RemoveExtrasLhsAttributes(singleRhsAttributeFds);
      return m_minimalCover.RemoveExtraFds(noExtraLhsAttributesFds);
    }

  }
}
