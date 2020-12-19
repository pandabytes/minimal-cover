using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MinimalCover.Xunit
{
  public class RelationTest 
  {
    public class FunctionalDependencyTestData
    {
      public IEnumerable<FunctionalDependency> Fds { get; set; }

      public IEnumerable<string> Attributes;
    }

    public static TheoryData<FunctionalDependencyTestData> FdsTestData =>
      new TheoryData<FunctionalDependencyTestData>
      {
        new FunctionalDependencyTestData() 
        {
          Fds = new FunctionalDependency[3] {
            new FunctionalDependency("A", "B,C"),
            new FunctionalDependency("B", "C"),
            new FunctionalDependency("A,B", "D")
          },
          Attributes = new string[4] { "A", "B", "C", "D" }
        },

        new FunctionalDependencyTestData()
        {
          Fds = new FunctionalDependency[5] {
            new FunctionalDependency("A", "D"),
            new FunctionalDependency("B,C", "A,D"),
            new FunctionalDependency("C", "B"),
            new FunctionalDependency("E", "A"),
            new FunctionalDependency("E", "D")
          },
          Attributes = new string[5] { "A", "B", "C", "D", "E" }
        },

        new FunctionalDependencyTestData()
        {
          Fds = new FunctionalDependency[11] {
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
          },
          Attributes = new string[6] { "A", "B", "C", "D", "E", "G"}
        }
      };

    [Theory]
    [MemberData(nameof(FdsTestData), MemberType = typeof(RelationTest))]
    public void Constructor_Test(FunctionalDependencyTestData expectedFds)
    {
      var relation = new Relation(expectedFds.Fds);
      Assert.True(relation.Fds.SetEquals(expectedFds.Fds));
      Assert.True(relation.Attributes.SetEquals(expectedFds.Attributes));
    }


  }
}
