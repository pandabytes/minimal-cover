using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using MinimalCover.Xunit.Data;
using MinimalCover.Core;

namespace MinimalCover.Xunit
{
  public class MinimalCoverTest
  {
    public static TheoryData<FunctionalDependency> FdsTheoryData =
      new TheoryData<FunctionalDependency>
      {
        new FunctionalDependency("A,B", "C,D"),
        new FunctionalDependency("A", "C,D"),
        new FunctionalDependency("E", "C,D,A"),
        new FunctionalDependency("A,B", "C"),
        new FunctionalDependency("A", "C")
      };

    public static TheoryData<ComputeClosureTestData> ComputeClosureTheoryData =
      new TheoryData<ComputeClosureTestData>
      {
        new ComputeClosureTestData()
        {
          Attribute = "A",
          Fds = new FunctionalDependency[3] {
            new FunctionalDependency("A", "D"),
            new FunctionalDependency("D", "B"),
            new FunctionalDependency("C", "D")
          },
          Closure = new AttributeSet("A,D,B")
        },
        new ComputeClosureTestData()
        {
          Attribute = "C",
          Fds = new FunctionalDependency[3] {
            new FunctionalDependency("A", "D"),
            new FunctionalDependency("D", "B"),
            new FunctionalDependency("C", "D")
          },
          Closure = new AttributeSet("B,D,C")
        },
        new ComputeClosureTestData()
        {
          Attribute = "E",
          Fds = new FunctionalDependency[6] {
            new FunctionalDependency("A", "D"),
            new FunctionalDependency("B,C", "A"),
            new FunctionalDependency("B,C", "D"),
            new FunctionalDependency("C", "B"),
            new FunctionalDependency("E", "D"),
            new FunctionalDependency("E", "A")
          },
          Closure = new AttributeSet("E,D,A")
        }
      };

    [Theory]
    [MemberData(nameof(FdsTheoryData),
            MemberType = typeof(MinimalCoverTest))]
    public void SingleAttributeRhs_Test(FunctionalDependency fd)
    {
      var fds = Core.MinimalCover.SingleAttributeRhs(fd);
      var allSingleRhs = fds.All(singleRhsFd => singleRhsFd.Right.Count == 1);
      Assert.True(allSingleRhs, "Not all fds have only 1 attribute on RHS");
    }

    [Theory]
    [MemberData(nameof(RelationTestData.TestData),
                MemberType = typeof(RelationTestData))]
    public void GetSingleAttributeRhsFds_Test(Relation relation)
    {
      var fds = Core.MinimalCover.GetSingleAttributeRhsFds(relation.Fds);
      var allSingleRhs = fds.All(fd => fd.Right.Count == 1);
      Assert.True(allSingleRhs, "Not all fds have only 1 attribute on RHS");
    }

    [Fact]
    public void ComputeClosure_NoSingleAttributeRhsException_Test()
    {
      var sampleFds = new FunctionalDependency[2] {
        new FunctionalDependency("A", "B,E"),
        new FunctionalDependency("C", "D"),
      };

      Assert.Throws<ArgumentException>(() => {
        Core.MinimalCover.ComputeClosure("A", sampleFds);
      });
    }

    [Theory]
    [MemberData(nameof(ComputeClosureTheoryData))]
    public void ComputeClosure_Test(ComputeClosureTestData testData)
    {
      var closure = Core.MinimalCover.ComputeClosure(testData.Attribute, testData.Fds);
      Assert.Equal(testData.Closure, closure);
    }



  }
}
