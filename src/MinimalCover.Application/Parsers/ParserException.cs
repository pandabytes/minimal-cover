using System;

namespace MinimalCover.Application.Parsers
{
  /// <summary>
  /// Thrown exception when <see cref="IParser"/>
  /// fails to parse a string of functional dependencies
  /// </summary>
  public class ParserException : Exception
  {
    public ParserException(string message) : base(message)
    { }

    public ParserException(string message, Exception innerEx) : base(message, innerEx)
    { }
  }
}
