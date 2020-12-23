using System;
using MinimalCover.Core;
using Xunit;

namespace MinimalCover.Xunit.Data
{
  public class RelationTestData
  {
    public static TheoryData<Relation> TestData =
      new TheoryData<Relation>
      {
        new Relation(
          new string[8] { "A", "B", "C", "D", "E", "F", "G", "H" },
          new FunctionalDependency[3] {
            new FunctionalDependency("A,B", "D,C"),
            new FunctionalDependency("D", "E,F,G"),
            new FunctionalDependency("F,G", "H")
          }
        ),

        new Relation(
          new string[4] { "A", "B", "C", "D" },
          new FunctionalDependency[3] {
            new FunctionalDependency("A", "B,C"),
            new FunctionalDependency("B", "C"),
            new FunctionalDependency("A,B", "D")
          }
        ),

        new Relation(
          new string[6] { "A", "B", "C", "D", "E", "G"},
          new FunctionalDependency[11] {
            new FunctionalDependency("A,B", "C"),
            new FunctionalDependency("C", "A"),
            new FunctionalDependency("B,C", "D"),
            new FunctionalDependency("A,C,D", "B,D"),
            new FunctionalDependency("D", "E"),
            new FunctionalDependency("D", "G"),
            new FunctionalDependency("B,E", "C"),
            new FunctionalDependency("C,G", "B"),
            new FunctionalDependency("C,G", "D"),
            new FunctionalDependency("C,E", "A"),
            new FunctionalDependency("C,E", "G")
          }
        )

      };

  }

  
}
