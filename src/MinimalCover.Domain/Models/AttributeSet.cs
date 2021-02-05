using System;
using System.Collections.Generic;
using System.Linq;
using MinimalCover.Domain.Core;

namespace MinimalCover.Domain.Models
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
    /// <param name="set">Set object that will converted to a readonly set</param>
    public AttributeSet(ISet<string> set) : base(set)
    { }
  }
}
