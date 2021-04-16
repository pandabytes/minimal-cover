using System;

namespace MinimalCover.Application.Parsers.Settings
{
  /// <summary>
  /// 
  /// </summary>
  public class ParserSettings
  {
    public static readonly string SectionPath = "ParsersSettings";

    public TextParserSettings? TextParser { get; init; }

    public JsonParserSettings? JsonParser { get; init; }
  }
}
