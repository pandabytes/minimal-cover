using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace MinimalCover
{
  public class Relation
  {
    public ReadOnlySet<FunctionalDependency> Fds { get; }

    public AttributeSet Attributes { get; }

    public Relation(IEnumerable<string> attributes, IEnumerable<FunctionalDependency> fds)
    {
      var fdsSet = new HashSet<FunctionalDependency>(fds);
      Fds = new ReadOnlySet<FunctionalDependency>(fdsSet);

      var attrbSet = new HashSet<string>(attributes);
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

      // Get all attributes
      var attributes = new HashSet<string>();
      foreach (var fd in Fds)
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

      Attributes = new AttributeSet(attributes);
    }

    public override string ToString() => $"Relation({string.Join(',', Attributes)})";
    
  }
}
