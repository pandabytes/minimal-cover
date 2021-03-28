using System;
using System.Collections.Generic;
using MinimalCover.Domain.Models;

namespace MinimalCover.Application.Parsers
{
  /// <summary>
  /// Available formats that can be parsed by
  /// <see cref="IParser"/>
  /// </summary>
  public enum ParseFormat
  {
    Text,
    Json,
    Yaml
  }

  /// <summary>
  /// A delegate used to get a parser based on the given parse format
  /// </summary>
  /// <param name="format">Parse format</param>
  /// <exception cref="NotSupportedException">Thrown when a format is not supported yet</exception>
  /// <returns>A <see cref="IParser"/> object</returns>
  public delegate IParser GetParser(ParseFormat format);

  /// <summary>
  /// Interface for parser class that parses one of the
  /// string formats defined in <see cref="ParseFormat"/>
  /// </summary>
  public interface IParser
  {
    /// <summary>
    /// Specify which format the parser will be parsing
    /// </summary>
    ParseFormat Format { get; }

    /// <summary>
    /// Parse the given <paramref name="value"/> into a set of
    /// <see cref="FunctionalDependency"/>
    /// </summary>
    /// <param name="value">The string value to parse</param>
    /// <exception cref="ParserException">
    /// Thrown when parsing <paramref name="value"/> fails
    /// </exception>
    /// <returns>Set of parsed <see cref="FunctionalDependency"/></returns>
    ISet<FunctionalDependency> Parse(string value);
  }
}
