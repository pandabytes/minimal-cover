using System.Collections.Generic;
using MinimalCover.Core;

namespace MinimalCover.Xunit.Core.Data
{
  public class ParsedFdsTestData
  {
    public string Value { get; set; }

    public ISet<FunctionalDependency> ExpectedFds { get; set; }
  }
}
