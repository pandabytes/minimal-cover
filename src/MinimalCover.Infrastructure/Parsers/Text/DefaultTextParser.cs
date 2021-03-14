using System;
using System.Linq;
using System.Collections.Generic;

using MinimalCover.Domain.Core;
using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Parsers.Settings;

namespace MinimalCover.Infrastructure.Parsers.Text
{
  /// <summary>
  /// Default implementation of <see cref="TextParser"/>
  /// </summary>
  internal class DefaultTextParser : TextParser
  {
    /// <inheritdoc/>
    public DefaultTextParser(TextParserSettings settings)
      : base(settings)
    {}

    /// <summary>
    /// Get attributes from the given string and separator.
    /// </summary>
    /// <example>
    /// "A,B,C" and "," ==> {"A", "B", "C"}
    /// </example>
    /// <param name="value">
    /// String containing attributes separated by <paramref name="sep"/>
    /// </param>
    /// <param name="sep">Separator</param>
    /// <returns>Collection of attributes</returns>
    protected static IEnumerable<string> GetAttributesWithSep(string value, string sep) => value.Split(sep).Select(a => a.Trim());

    /// <inheritdoc/>
    public override ISet<FunctionalDependency> Parse(string value)
    {
      // Get each fd string
      var fdStrings = value.Split(FdSeparator)
                           .Where(fd => !string.IsNullOrEmpty(fd))
                           .Select(fd => fd.Trim());

      const string InvalidFdFormat = "{0}. Invalid functional dependency \"{1}\"";
      var fds = fdStrings.Select(fd => {
        // Check if each fd string can be parsed into "left" and "right"
        var fdTokens = fd.Split(LeftRightSeparator);
        if (fdTokens.Length != 2)
        {
          var message = $"LHS and RHS must be separated by \"{LeftRightSeparator}\"";
          throw new ParserException(string.Format(InvalidFdFormat, message, fd));
        }

        // Check if left or right is empty
        var leftAttrbStr = fdTokens[0];
        var rightAttrbStr = fdTokens[1];
        if (string.IsNullOrWhiteSpace(leftAttrbStr) || string.IsNullOrWhiteSpace(rightAttrbStr))
        {
          throw new ParserException(string.Format(InvalidFdFormat, EmptyLhsOrRhsMessage, fd));
        }

        var leftAttributes = GetAttributesWithSep(leftAttrbStr, AttributeSeparator).ToHashSet();
        var rightAttributes = GetAttributesWithSep(rightAttrbStr, AttributeSeparator).ToHashSet();
        return new FunctionalDependency(leftAttributes, rightAttributes);
      }).ToHashSet();

      return new ReadOnlySet<FunctionalDependency>(fds);
    }

  }
}
