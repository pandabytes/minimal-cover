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
    /// Construct the text parser with optional separators
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Throw when at least 1 parameter is null
    /// </exception>
    /// <param name="attrbSep">attribute separator</param>
    /// <param name="fdSep">functional dependency separator</param>
    /// <param name="leftRightSep">LHS and RHS separator</param>
    public TextParser(string attrbSep, string fdSep, string leftRightSep)
    {
      if (string.IsNullOrEmpty(attrbSep) ||
          string.IsNullOrEmpty(fdSep) ||
          string.IsNullOrEmpty(leftRightSep))
      {
        throw new ArgumentException(InvalidSepsMessage);
      }

      AttributeSeparator = attrbSep;
      FdSeparator = fdSep;
      LeftRightSeparator = leftRightSep;
    }

    public TextParser(TextParserSettings settings) 
      : this(settings?.AttributeSeparator,
             settings?.FdSeparator,
             settings?.LeftRightSeparator)
    { }

    /// <summary>
    /// Interface method <see cref="IParser.Parse(string)"/>
    /// </summary>
    public abstract ISet<FunctionalDependency> Parse(string value);
  }
}
