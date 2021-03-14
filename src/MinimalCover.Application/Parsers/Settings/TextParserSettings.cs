using System;

namespace MinimalCover.Application.Parsers.Settings
{
  /// <summary>
  /// Represent the Text parser section in appsettings.json
  /// </summary>
  public class TextParserSettings
  {
    public static readonly string SectionPath = "Parsers:TextParser";

    public string AttributeSeparator { get; set; }

    public string FdSeparator { get; set; }

    public string LeftRightSeparator { get; set; }
  }
}
