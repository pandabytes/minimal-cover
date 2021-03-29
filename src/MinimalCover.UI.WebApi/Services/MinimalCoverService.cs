using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MinimalCover.Domain.Models;
using MinimalCover.Application;
using MinimalCover.Application.Algorithms;
using MinimalCover.Application.Parsers;

using Microsoft.Extensions.Logging;

namespace MinimalCover.UI.WebApi.Services
{
  public class MinimalCoverService
  {
    private readonly ILogger<MinimalCoverService> m_logger;
    private readonly MinimalCoverApp m_mcApp;
    private readonly GetParser m_getParser;

    public MinimalCoverService(ILogger<MinimalCoverService> logger,
                               IMinimalCover minimalCover,
                               GetParser getParser)
    {
      m_logger = logger;
      m_mcApp = new MinimalCoverApp(minimalCover);
      m_getParser = getParser;
    }

    public ISet<FunctionalDependency> FindMinimalCover(ParseFormat parseFormat, string value)
    {
      var parser = m_getParser(parseFormat);
      return m_mcApp.FindMinimalCover(value, parser);
    }

    public ISet<FunctionalDependency> FindMinimalCover(ISet<FunctionalDependency> fds)
      => m_mcApp.FindMinimalCover(fds);

  }
}
