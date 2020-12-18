using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace MinimalCover
{
  /// <summary>
  /// Contain a readonly set of attribute
  /// </summary>
  public class AttributeSet : ReadOnlySet<string>
  {
    /// <summary>
    /// Passed in set can still be update if there is a external reference to it.
    /// This constructor only stores a reference to <see cref="set"/>
    /// </summary>
    public AttributeSet(ISet<string> set) : base(set)
    {}

    public AttributeSet(string setStr, char separator = ',')
    {
      var tokens = setStr.Split(separator).Select(t => t.Trim());
      m_set = new HashSet<string>(tokens);
    }
  }
}
