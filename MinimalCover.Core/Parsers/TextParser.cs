using System;
using System.Collections.Generic;
using System.Linq;

namespace MinimalCover.Core.Parsers
{
  public static class TextParser
  {
    public static readonly char FdSeparator = ';';
    public static readonly string LeftRightSeparator = "-->";

    /// <summary>
    /// Parse the given <paramref name="value"/> into a set of
    /// <see cref="FunctionalDependency"/>
    /// </summary>
    /// <param name="value">The string value to parse</param>
    /// <returns>Set of parsed <see cref="FunctionalDependency"/></returns>
    public static ReadOnlySet<FunctionalDependency> Parse(string value)
    {
      var fdStrings = value.Split(FdSeparator)
                           .Where(fd => !string.IsNullOrEmpty(fd))
                           .Select(fd => fd.Trim());

      var fds = fdStrings.Select(fd => {
        var fdTokens = fd.Split(LeftRightSeparator);
        if (fdTokens.Length != 2)
        {
          throw new ArgumentException($"LHS and RHS must be separated by '{LeftRightSeparator}'");
        }

        var left = fdTokens[0];
        var right = fdTokens[1];
        if (string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(right))
        {
          throw new ArgumentException("LHS and RHS must not be empty");
        }
        return new FunctionalDependency(left, right);
      }).ToHashSet();

      return new ReadOnlySet<FunctionalDependency>(fds);
    }

  }
}
