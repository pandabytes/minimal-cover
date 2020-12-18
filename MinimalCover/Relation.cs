using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace MinimalCover
{
  public class Relation
  {
    public IReadOnlyCollection<FunctionalDependency> Fds { get; }

    public ReadOnlySet<string> Attributes { get; }

    public Relation(IEnumerable<FunctionalDependency> fds)
    {
      var fdsList = (IList<FunctionalDependency>)fds;
      Fds = new ReadOnlyCollection<FunctionalDependency>(fdsList);

      // Get all attributes
      var attributes = new HashSet<string>();
      foreach (var fd in Fds)
      {
        foreach (var attribute in fd.Left)
        {
          attributes.Add(attribute);
        }

        foreach (var attribute in fd.Right)
        {
          attributes.Add(attribute);
        }
      }

      Attributes = new ReadOnlySet<string>(attributes);
    }


  }
}
