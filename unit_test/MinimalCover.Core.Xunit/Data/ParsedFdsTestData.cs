using System.Collections.Generic;

namespace MinimalCover.Core.Data.Xunit
{
  public class ParsedFdsTestData
  {
    public string Value { get; set; }

    public ISet<FunctionalDependency> ExpectedFds { get; set; }
  }
}
