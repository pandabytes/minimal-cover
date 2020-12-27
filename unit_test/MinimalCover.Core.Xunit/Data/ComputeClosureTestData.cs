using System;
using System.Collections.Generic;
using MinimalCover.Core;

namespace MinimalCover.Core.Xunit.Data
{
  public class ComputeClosureTestData
  {
    public string Attribute { get; set; }

    public IEnumerable<FunctionalDependency> Fds { get; set; }

    public AttributeSet Closure { get; set; }
  }
}
