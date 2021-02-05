using System;
using System.Collections.Generic;
using System.Linq;
using MinimalCover.Domain.Models;

namespace MinimalCover.UnitTests.Utils
{
  public static class FuncDepUtils
  {
    /// <summary>
    /// Easily construct a <see cref="FunctionalDependency"/> object 
    /// from strings with separator. This method uses
    /// <see cref="HashSet{T}"/> implementation to
    /// store the attributes.
    /// </summary>
    /// <param name="left">left attributes as string</param>
    /// <param name="right">right attributes as string</param>
    /// <param name="sep">Separator that separate left and right</param>
    /// <returns>A <see cref="FunctionalDependency"/> object </returns>
    public static FunctionalDependency ConstructFdFromString(string left, string right, string sep = ",")
    {
      var leftAttrbs = left.Split(sep).Select(a => a.Trim()).ToHashSet();
      var rightAttrbs = right.Split(sep).Select(a => a.Trim()).ToHashSet();
      return new FunctionalDependency(leftAttrbs, rightAttrbs);
    }
  }
}
