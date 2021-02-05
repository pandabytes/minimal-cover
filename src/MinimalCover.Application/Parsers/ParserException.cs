using System;

namespace MinimalCover.Application.Parsers
{
  /// <summary>
  /// 
  /// </summary>
  public class ParserException : Exception
  {
    public ParserException(string message) : base(message)
    { }

    public ParserException(string message, Exception innerEx) : base(message, innerEx)
    { }
  }
}
