using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace MinimalCover.Core
{
  public class Relation
  {
    public ReadOnlySet<FunctionalDependency> Fds { get; }

    public AttributeSet Attributes { get; }

    public Relation(IEnumerable<string> attributes, IEnumerable<FunctionalDependency> fds)
    {
      var fdsSet = new HashSet<FunctionalDependency>(fds);
      Fds = new ReadOnlySet<FunctionalDependency>(fdsSet);

      // Union the collection of attributes and the collection
      // of attributes found in the collection of functional dependencies
      var attrbSet = new HashSet<string>(attributes);
      attrbSet.UnionWith(GetAttributes(fds));
      Attributes = new AttributeSet(attrbSet);
    }

    /// <summary>
    /// This constructor assumes all attributes are captured in
    /// the given collection of functional dependencies.
    /// </summary>
    /// <param name="fds"></param>
    public Relation(IEnumerable<FunctionalDependency> fds)
    {
      var fdsSet = new HashSet<FunctionalDependency>(fds);
      Fds = new ReadOnlySet<FunctionalDependency>(fdsSet);
      Attributes = new AttributeSet(GetAttributes(fds));
    }

    private static ISet<string> GetAttributes(IEnumerable<FunctionalDependency> fds)
    {
      // Get all attributes
      var attributes = new HashSet<string>();
      foreach (var fd in fds)
      {
        // Grab all attributes in each functional dependency
        foreach (var attribute in fd.Left)
        {
          attributes.Add(attribute);
        }

        foreach (var attribute in fd.Right)
        {
          attributes.Add(attribute);
        }
      }
      return attributes;
    }

    public override string ToString() => $"Relation({string.Join(',', Attributes)})";
    
  }
}
