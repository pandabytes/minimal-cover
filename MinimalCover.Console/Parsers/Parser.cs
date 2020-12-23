using MinimalCover.Core;
using System;
using System.Collections.Generic;

namespace MinimalCover.Console.Parsers
{
  public abstract class Parser : IParser
  {
    /// <summary>
    /// Keep a reference to the modify-able set
    /// </summary>
    protected HashSet<FunctionalDependency> m_parsedFds;

    public ReadOnlySet<FunctionalDependency> ParsedFds { get; }

    protected Parser()
    {
      m_parsedFds = new HashSet<FunctionalDependency>();
      ParsedFds = new ReadOnlySet<FunctionalDependency>(m_parsedFds);
    }

    public virtual void Parse()
    {
      throw new NotImplementedException();
    }
  }
}
