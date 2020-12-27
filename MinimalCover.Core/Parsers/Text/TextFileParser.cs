using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MinimalCover.Core;

namespace MinimalCover.Core.Parsers.Text
{
  /// <summary>
  /// </summary>
  /// <remarks>
  /// The code in this class may be similar to <see cref="Cli.CliParser"/>.
  /// But since these are different parsers, they can potentially
  /// have different ways to parse in the future. So it is fine if
  /// the code is be duplicated in these 2 classes for now
  /// </remarks>
  public class TextFileParser : FileParser
  {
    protected const char FdSeparator = ';';
    protected const string LeftRightSeparator = "-->";

    public TextFileParser(string filePath) : base(filePath) { }

    public override void Parse()
    {
      if (ParsedFds.Count > 0)
      {
        throw new InvalidOperationException($"{FilePath} is already parsed");
      }

      using (var file = new StreamReader(FilePath))
      {
        int lineCount = 0;
        string line;
        while ( (line = file.ReadLine()) != null )
        {
          lineCount++;

          var fdStrings = line.Split(FdSeparator)
                              .Where(fd => !string.IsNullOrEmpty(fd));

          var fds = fdStrings.Select(fd => {
            var fdTokens = fd.Split(LeftRightSeparator);
            if (fdTokens.Length != 2)
            {
              throw new FileParserException(lineCount, $"Each functional dependency must be separated by '{LeftRightSeparator}'");
            }

            var left = fdTokens[0];
            var right = fdTokens[1];
            if (string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(right))
            {
              throw new FileParserException(lineCount, "LHS and RHS must not be empty");
            }
            return new FunctionalDependency(fdTokens[0], fdTokens[1]);
          });

          m_parsedFds.UnionWith(fds);
        }
      }
    }

  }
}
