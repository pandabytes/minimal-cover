using System;
using System.Linq;
using System.Collections.Generic;
using MinimalCover.Domain.Core;
using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers;

namespace MinimalCover.Infrastructure.Parsers
{
  public class TextParser : IParser
  {
    public static readonly string EmptyLhsOrRhsMessage = "LHS and RHS must not be empty";

    /// <summary>
    /// The separator between 2 attributes
    /// </summary>
    public string AttributeSeparator { get; }

    /// <summary>
    /// The separator between 2 functional dependencies
    /// </summary>
    public string FdSeparator { get; }

    /// <summary>
    /// The separator between the LHS and RHS of
    /// a functional dependency
    /// </summary>
    public string LeftRightSeparator { get; }

    /// <summary>
    /// Return <see cref="ParseFormat.Text"/>
    /// </summary>
    ParseFormat IParser.Format
    {
      get { return ParseFormat.Text; }
    }

    /// <summary>
    /// Construct the text parser with optional separators
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Throw when at least 1 parameter is null
    /// </exception>
    /// <param name="attrbSep">attribute separator</param>
    /// <param name="fdSep">functional dependency separator</param>
    /// <param name="leftRightSep">LHS and RHS separator</param>
    public TextParser(string attrbSep = ",", string fdSep = ";", string leftRightSep = "-->")
    {
      if (string.IsNullOrEmpty(attrbSep) || 
          string.IsNullOrEmpty(fdSep) || 
          string.IsNullOrEmpty(leftRightSep))
      {
        throw new ArgumentException($"All parameters must be non-empty and non-null strings");
      }

      AttributeSeparator = attrbSep;
      FdSeparator = fdSep;
      LeftRightSeparator = leftRightSep;
    }

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
    private static IEnumerable<string> GetAttributesWithSep(string value, string sep) => value.Split(sep).Select(a => a.Trim());

    /// <summary>
    /// Parse the given <paramref name="value"/> into a set of
    /// <see cref="FunctionalDependency"/>
    /// </summary>
    /// <param name="value">The string value to parse</param>
    /// <returns>Set of parsed <see cref="FunctionalDependency"/></returns>
    ISet<FunctionalDependency> IParser.Parse(string value)
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
