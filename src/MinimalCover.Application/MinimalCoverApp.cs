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
  public abstract class MinimalCoverApp
  {
    protected readonly IMinimalCover m_minimalCover;

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
    /// <returns>A set of <see cref="FunctionalDependency"/></returns>
    public abstract ISet<FunctionalDependency> FindMinimalCover(IParser parser, string value);

    /// <summary>
    /// Find the minimal cover
    /// </summary>
    /// <param name="fds">Set of <see cref="FunctionalDependency"/></param>
    /// <returns></returns>
    public abstract ISet<FunctionalDependency> FindMinimalCover(ISet<FunctionalDependency> fds);

  }
}
