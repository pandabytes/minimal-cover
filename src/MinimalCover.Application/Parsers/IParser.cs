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
  /// Delegate that defines signature to retrieve
  /// an implementation of <see cref="IParser"/>
  /// </summary>
  /// <param name="format">Format</param>
  /// <returns>The parser that supports the given format</returns>
  public delegate IParser GetParser(ParseFormat format);

  /// <summary>
  /// Interface for parser class that parses one of the
  /// string formats defined in <see cref="ParseFormat"/>
  /// </summary>
  public interface IParser
  {
    ParseFormat Format { get; }

    ISet<FunctionalDependency> Parse(string value);
  }
}
