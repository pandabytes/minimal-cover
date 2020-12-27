using System;
using System.IO;

namespace MinimalCover.Core.Parsers
{
  public abstract class FileParser : Parser
  {
    public string FilePath { get; }

    public FileParser(string filePath) : base()
    {
      if (!File.Exists(filePath))
      {
        throw new ArgumentException($"{nameof(filePath)} must be a valid path. Got {filePath}");
      }
      FilePath = filePath;
    }
  }
}
