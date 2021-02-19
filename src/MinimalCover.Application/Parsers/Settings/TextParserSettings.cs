using System;

namespace MinimalCover.Application.Parsers.Settings
{
  public class TextParserSettings
  {
    public static readonly string TextParser = "TextParser";

    public string AttributeSeparator { get; set; }

    public string FdSeparator { get; set; }

    public string LeftRightSeparator { get; set; }
  }
}
