using System;

namespace MinimalCover.Application.Parsers.Settings
{
  /// <summary>
  /// Represent the Text parser section in appsettings.json
  /// </summary>
  public class TextParserSettings
  {
    public static readonly string SectionPath = "Parsers:TextParser";

    protected string m_attributeSeparator = string.Empty;
    protected string m_fdSeparator = string.Empty;
    protected string m_leftRightSeparator = string.Empty;

    public string AttributeSeparator
    {
      get { return m_attributeSeparator; }
      init { m_attributeSeparator = value; }
    }

    public string FdSeparator
    {
      get { return m_fdSeparator; }
      init { m_fdSeparator = value; }
    }

    public string LeftRightSeparator
    {
      get { return m_leftRightSeparator; }
      init { m_leftRightSeparator = value; }
    }
  }
}
