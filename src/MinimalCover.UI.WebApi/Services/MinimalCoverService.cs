using System;
using System.Collections.Generic;
using System.Linq;

using MinimalCover.Application;
using MinimalCover.Domain.Models;
using MinimalCover.Application.Algorithms;
using MinimalCover.Application.Parsers;

using Microsoft.Extensions.Logging;

namespace MinimalCover.UI.WebApi.Services
{
  /// <summary>
  /// Class providing the minimal cover service
  /// </summary>
  public class MinimalCoverService
  {
    private readonly MinimalCoverApp m_mcApp;
    private readonly GetParser m_getParser;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="minimalCoverApp">Minimal cover algorithm</param>
    /// <param name="getParser">Get parser delegate</param>
    public MinimalCoverService(MinimalCoverApp minimalCoverApp, GetParser getParser)
    {
      m_mcApp = minimalCoverApp;
      m_getParser = getParser;
    }

    /// <summary>
    /// Find the minmal cover given <paramref name="parseFormat"/> and the value string
    /// </summary>
    /// <param name="parseFormat">Format of <paramref name="value"/></param>
    /// <param name="value">Value to be parsed into a set of <see cref="FunctionalDependency"/></param>
    /// <exception cref="NotSupportedException">Thrown <paramref name="parseFormat"/> is not currently supported</exception>
    /// <returns>A set of <see cref="FunctionalDependency"/> representing the minimal cover</returns>
    public ISet<FunctionalDependency> FindMinimalCover(ParseFormat parseFormat, string value)
    {
      var parser = m_getParser(parseFormat);
      return m_mcApp.FindMinimalCover(parser, value);
    }

    /// <summary>
    /// Find the minmal cover given <paramref name="fds"/>
    /// </summary>
    /// <param name="fds">Format of <paramref name="value"/></param>
    /// <returns>A set of <see cref="FunctionalDependency"/> representing the minimal cover</returns>
    public ISet<FunctionalDependency> FindMinimalCover(ISet<FunctionalDependency> fds)
      => m_mcApp.FindMinimalCover(fds);
  }
}
