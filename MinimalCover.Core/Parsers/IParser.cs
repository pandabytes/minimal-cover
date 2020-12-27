using System;
using System.Collections.Generic;
using MinimalCover.Core;

namespace MinimalCover.Core.Parsers
{
  public interface IParser
  {
    ReadOnlySet<FunctionalDependency> ParsedFds { get; }

    void Parse();

  }
}
