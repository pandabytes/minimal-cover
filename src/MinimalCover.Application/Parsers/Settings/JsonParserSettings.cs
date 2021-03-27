using System;

namespace MinimalCover.Application.Parsers.Settings
{
  /// <summary>
  /// Represent the JSON parser section in appsettings.json
  /// </summary>
  public class JsonParserSettings
  {
    public static readonly string SectionPath = "Parsers:JsonParser";

    protected string m_schemaFilePath = string.Empty;

    public string SchemaFilePath
    {
      get { return m_schemaFilePath; }
      init { m_schemaFilePath = value; }
    }
  }
}
