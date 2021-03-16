using System;

namespace MinimalCover.Application.Parsers.Settings
{
  /// <summary>
  /// Represent the Text parser section in appsettings.json
  /// </summary>
  public class TextParserSettings
  {
    public static readonly string SectionPath = "Parsers:TextParser";

    public string AttributeSeparator { get; init; }

    public string FdSeparator { get; init; }

    public string LeftRightSeparator { get; init; }
  }
}
