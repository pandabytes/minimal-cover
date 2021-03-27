using System;
using System.Linq;
using System.Collections.Generic;
using MinimalCover.Domain.Core;
using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers.Settings;

namespace MinimalCover.Application.Parsers
{
  /// <summary>
  /// This class is responsible for parsing functional dependencies
  /// that are in <see cref="ParseFormat.Text"/>
  /// </summary>
  public abstract class TextParser : IParser
  {
    public static readonly string EmptyLhsOrRhsMessage = "LHS and RHS must not be empty";
    public static readonly string InvalidSepsMessage = "All separators must be non-empty and non-null strings";

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
    ParseFormat IParser.Format { get { return ParseFormat.Text; } }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Throw when <paramref name="settings"/> is null or
    /// the properties in <paramref name="settings"/> are null or not empty
    /// </exception>
    /// <param name="settings">Text parser settings</param>
    public TextParser(TextParserSettings settings)
    {
      if (string.IsNullOrEmpty(settings?.AttributeSeparator) ||
          string.IsNullOrEmpty(settings?.FdSeparator) ||
          string.IsNullOrEmpty(settings?.LeftRightSeparator))
      {
        throw new ArgumentException(InvalidSepsMessage);
      }

      AttributeSeparator = settings.AttributeSeparator;
      FdSeparator = settings.FdSeparator;
      LeftRightSeparator = settings.LeftRightSeparator;
    }

    /// <summary>
    /// Interface method <see cref="IParser.Parse(string)"/>
    /// </summary>
    public abstract ISet<FunctionalDependency> Parse(string value);
  }
}
