using System;
using System.Collections.Generic;
using System.Linq;

namespace MinimalCover.Core
{
  /// <summary>
  /// Represent a readonly set of attributes
  /// </summary>
  public class AttributeSet : ReadOnlySet<string>
  {
    /// <summary>
    /// Passed in set can still be update if there is a external reference to it.
    /// This constructor only stores a reference to <see cref="set"/>
    /// </summary>
    public AttributeSet(ISet<string> set) : base(set)
    {}

    /// <summary>
    /// Construct an attribute set by providing a string and separator
    /// </summary>
    /// <remarks>
    /// Example: "A,B,C" and "," ==> {"A", "B", "C"}
    /// </remarks>
    /// <param name="setStr">The attributes with separator included in 1 string</param>
    /// <param name="separator">The separator to split the string</param>
    public AttributeSet(string setStr, char separator = ',')
    {
      var tokens = setStr.Split(separator).Select(t => t.Trim());
      m_set = new HashSet<string>(tokens);
    }
  }
}
