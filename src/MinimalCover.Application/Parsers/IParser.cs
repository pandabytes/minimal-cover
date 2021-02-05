using System;
using System.Collections.Generic;
using MinimalCover.Domain.Models;

namespace MinimalCover.Application.Parsers
{
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
    ParseFormat Format { get; }

    ISet<FunctionalDependency> Parse(string value);
  }
}
