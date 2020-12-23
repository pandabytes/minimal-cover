using System;
using System.Collections.Generic;
using System.Text;

namespace MinimalCover.Console.Parsers
{
  public class FileParserException : Exception
  {
    public int LineNumber { get; }

    public override string Message => $"{base.Message}. Line: {LineNumber}";

    public FileParserException() : base() { }

    public FileParserException(string message) : base(message) { }

    public FileParserException(string message, Exception innerEx) : base(message, innerEx) { }

    public FileParserException(int lineNumber)
    {
      LineNumber = lineNumber;
    }

    public FileParserException(int lineNumber, string message) : base(message)
    {
      LineNumber = lineNumber;
    }

    public FileParserException(int lineNumber, string message, Exception innerEx) : base(message, innerEx)
    {
      LineNumber = lineNumber;
    }
  }
}
