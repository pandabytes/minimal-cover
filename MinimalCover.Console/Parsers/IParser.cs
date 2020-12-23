using System;
using System.Collections.Generic;
using MinimalCover.Core;

namespace MinimalCover.Console.Parsers
{
  public interface IParser
  {
    ReadOnlySet<FunctionalDependency> ParsedFds { get; }

    void Parse();
  }
}
