using System;
using System.Collections.Generic;

namespace MinimalCover.Core.Parsers
{
  /// <summary>
  /// Specify the currently supported list
  /// of formats that can be parsed
  /// </summary>
  public enum InputFormat
  {
    Text,
    Json,
    Yaml
  }

  /// <summary>
  /// Main class to call the <see cref="Parse(InputFormat, string)"/> method,
  /// in which will use the correct parser to do the actual parsing
  /// </summary>
  public static class Parser
  {
    /// <summary>
    /// This delegate serves as a common parser interface for 
    /// different formats, specifically listed in <see cref="InputFormat"/>
    /// </summary>
    /// <param name="value">Value to be parsed</param>
    /// <returns>Readonly set of functional dependencies</returns>
    public delegate ReadOnlySet<FunctionalDependency> ParseMethod(string value);

    /// <summary>
    /// Parse the given <paramref name="value"/> into a set of
    /// <see cref="FunctionalDependency"/>
    /// </summary>
    /// <param name="value">The string value to parse</param>
    /// <returns>Set of parsed <see cref="FunctionalDependency"/></returns>
    public static ISet<FunctionalDependency> Parse(InputFormat inputFormat, string value)
    {
      ParseMethod parseMethod;
      switch (inputFormat)
      {
        case InputFormat.Text:
          parseMethod = TextParser.Parse;
          break;
        case InputFormat.Json:
          parseMethod = JsonParser.Parse;
          break;
        case InputFormat.Yaml:
          parseMethod = YamlParser.Parse;
          break;
        default:
          throw new NotSupportedException($"Format \"{inputFormat}\" is not supported");
      }

      return parseMethod(value);
    }
  }
}
