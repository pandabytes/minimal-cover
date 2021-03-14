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
    /// <returns>Set of parsed <see cref="FunctionalDependency"/></returns>
    ISet<FunctionalDependency> Parse(string value);
  }
}
