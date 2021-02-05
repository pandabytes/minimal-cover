using System;
using System.Collections.Generic;
using System.Linq;
using MinimalCover.Domain.Models;

namespace MinimalCover.Infrastructure.UnitTests.Utils
{
  public static class FuncDepUtils
  {
    public static FunctionalDependency ConstructFdFromString(string left, string right, string sep)
    {
      var leftAttrbs = left.Split(sep).Select(a => a.Trim()).ToHashSet();
      var rightAttrbs = right.Split(sep).Select(a => a.Trim()).ToHashSet();
      return new FunctionalDependency(leftAttrbs, rightAttrbs);
    }
  }
}
