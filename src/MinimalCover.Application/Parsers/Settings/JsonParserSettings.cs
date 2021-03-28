using System;

namespace MinimalCover.Application.Parsers.Settings
{
  /// <summary>
  /// Represent the JSON parser section in appsettings.json
  /// </summary>
  public class JsonParserSettings
  {
    public static readonly string SectionPath = "Parsers:JsonParser";

    public string? SchemaFilePath { get; init; }

  }
}
