using System;
using System.Linq;
using MinimalCover.Core;

namespace MinimalCover.Core.Parsers.Cli
{
  /// <summary>
  /// </summary>
  /// <remarks>
  /// The code in this class may be similar to <see cref="Text.TextFileParser"/>.
  /// But since these are different parsers, they can potentially
  /// have different ways to parse in the future. So it is fine if
  /// the code is be duplicated in these 2 classes for now
  /// </remarks>
  public class CliParser : Parser
  {
    protected const char FdSeparator = ';';
    protected const string LeftRightSeparator = "-->";

    protected string m_fdsString;

    public CliParser(string fdsString) : base()
    {
      m_fdsString = fdsString;
    }

    public override void Parse()
    {
      var fdStrings = m_fdsString.Split(FdSeparator)
                                 .Where(fd => !string.IsNullOrEmpty(fd));

      var fds = fdStrings.Select(fd => {
        var fdTokens = fd.Split(LeftRightSeparator);
        if (fdTokens.Length != 2)
        {
          throw new ArgumentException($"Each functional dependency must be separated by '{LeftRightSeparator}'");
        }

        var left = fdTokens[0];
        var right = fdTokens[1];
        if (string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(right))
        {
          throw new ArgumentException("LHS and RHS must not be empty");
        }
        return new FunctionalDependency(fdTokens[0], fdTokens[1]);
      });

      m_parsedFds.UnionWith(fds);
    }
  }
}
