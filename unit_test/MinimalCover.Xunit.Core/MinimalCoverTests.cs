using System;
using System.Collections.Generic;
using Xunit;
using MinimalCover.Core;
using MinimalCover.Xunit.Core.Data;

namespace MinimalCover.Xunit.Core
{
  public class MinimalCoverTests
  {
    public class ClosureTestData
    {
      public string Attributes { get; set; }

      public ISet<FunctionalDependency> Fds { get; set; }

      public AttributeSet Closure { get; set; }
    }

    public class FdsTestData
    {
      public ISet<FunctionalDependency> Fds { get; set; }

      public ISet<FunctionalDependency> ExpectedFds { get; set; }
    }

    /// <summary>
    /// Some sample <see cref="FunctionalDependency"/> objects
    /// </summary>
    public static TheoryData<FunctionalDependency> FdsTheoryData =
      new TheoryData<FunctionalDependency>
      {
        new FunctionalDependency("A,B", "C,D"),
        new FunctionalDependency("A", "C,D"),
        new FunctionalDependency("E", "C,D,A"),
        new FunctionalDependency("A,B", "C"),
        new FunctionalDependency("A", "C")
      };

    public static TheoryData<ClosureTestData> ComputeClosureTheoryData =
      new TheoryData<ClosureTestData>
      {
        new ClosureTestData()
        {
          Attributes = "A",
          Fds = new HashSet<FunctionalDependency> {
            new FunctionalDependency("A", "D"),
            new FunctionalDependency("D", "B"),
            new FunctionalDependency("C", "D")
          },
          Closure = new AttributeSet("A,D,B")
        },
        new ClosureTestData()
        {
          Attributes = "C",
          Fds = new HashSet<FunctionalDependency> {
            new FunctionalDependency("A", "D"),
            new FunctionalDependency("D", "B"),
            new FunctionalDependency("C", "D")
          },
          Closure = new AttributeSet("B,D,C")
        },
        new ClosureTestData()
        {
          Attributes = "E",
          Fds = new HashSet<FunctionalDependency> {
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

    public static TheoryData<FdsTestData> RemoveExtrasAttributesLhsTheoryData =
      new TheoryData<FdsTestData>()
      {
        new FdsTestData()
        {
          Fds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A,B", "C"),
            new FunctionalDependency("C", "A"),
            new FunctionalDependency("B,C", "D"),
            new FunctionalDependency("A,C,D", "B"),
            new FunctionalDependency("A,C,D", "D"),
            new FunctionalDependency("D", "E"),
            new FunctionalDependency("D", "G"),
            new FunctionalDependency("B,E", "C"),
            new FunctionalDependency("C,G", "B"),
            new FunctionalDependency("C,G", "D"),
            new FunctionalDependency("C,E", "A"),
            new FunctionalDependency("C,E", "G")
          },
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A,B", "C"),
            new FunctionalDependency("C", "A"),
            new FunctionalDependency("B,C", "D"),
            new FunctionalDependency("C,D", "B"),
            new FunctionalDependency("C,D", "D"),
            new FunctionalDependency("D", "E"),
            new FunctionalDependency("D", "G"),
            new FunctionalDependency("B,E", "C"),
            new FunctionalDependency("C,G", "B"),
            new FunctionalDependency("C,G", "D"),
            new FunctionalDependency("C,E", "A"),
            new FunctionalDependency("C,E", "G")
          }
        },
        new FdsTestData()
        {
          Fds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A", "D"),
            new FunctionalDependency("B,C", "D"),
            new FunctionalDependency("B,C", "A"),
            new FunctionalDependency("C", "B"),
            new FunctionalDependency("E", "D"),
            new FunctionalDependency("E", "A")
          },
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A", "D"),
            new FunctionalDependency("C", "D"),
            new FunctionalDependency("C", "A"),
            new FunctionalDependency("C", "B"),
            new FunctionalDependency("E", "D"),
            new FunctionalDependency("E", "A")
          }
        }
      };

    public static TheoryData<FdsTestData> RemoveExtraFdsTheoryData =
      new TheoryData<FdsTestData>()
      {
        new FdsTestData()
        {
          Fds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A", "D"),
            new FunctionalDependency("C", "D"),
            new FunctionalDependency("C", "A"),
            new FunctionalDependency("C", "B"),
            new FunctionalDependency("E", "D"),
            new FunctionalDependency("E", "A")
          },
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A", "D"),
            new FunctionalDependency("C", "A"),
            new FunctionalDependency("C", "A"),
            new FunctionalDependency("C", "B"),
            new FunctionalDependency("E", "A")
          }
        },
        new FdsTestData()
        {
          Fds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A,B", "C"),
            new FunctionalDependency("C", "A"),
            new FunctionalDependency("B,C", "D"),
            new FunctionalDependency("C,D", "B"),
            new FunctionalDependency("C,D", "D"),
            new FunctionalDependency("D", "E"),
            new FunctionalDependency("D", "G"),
            new FunctionalDependency("B,E", "C"),
            new FunctionalDependency("C,G", "B"),
            new FunctionalDependency("C,G", "D"),
            new FunctionalDependency("C,E", "A"),
            new FunctionalDependency("C,E", "G")
          },
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            new FunctionalDependency("A,B", "C"),
            new FunctionalDependency("C", "A"),
            new FunctionalDependency("B,C", "D"),
            new FunctionalDependency("D", "E"),
            new FunctionalDependency("D", "G"),
            new FunctionalDependency("B,E", "C"),
            new FunctionalDependency("C,G", "B"),
            new FunctionalDependency("C,E", "G")
          }
        }
      };

    [Theory]
    [MemberData(nameof(RelationTestData.TestData),
                MemberType = typeof(RelationTestData))]
    public void GetSingleAttributeRhsFds_SimpleCall_AllFdsHaveSingleAttributeRhs(Relation relation)
    {
      var fds = MinimalCover.Core.MinimalCover.GetSingleAttributeRhsFds(relation.Fds);
      Action<FunctionalDependency> isSingleRhs = fd => {
        Assert.Single(fd.Right);
      };
      Assert.All(fds, isSingleRhs);
    }

    [Fact]
    public void ComputeClosure_HaveNoSingleRhsFds_ThrowsArgumentException()
    {
      var sampleFds = new HashSet<FunctionalDependency> {
        new FunctionalDependency("A", "B,E"),
        new FunctionalDependency("C", "D"),
      };

      Assert.Throws<ArgumentException>(() => MinimalCover.Core.MinimalCover.ComputeClosure("A", sampleFds));
    }

    [Theory]
    [MemberData(nameof(ComputeClosureTheoryData))]
    public void ComputeClosure_SimpleCall_MatchesExpectedClosure(ClosureTestData testData)
    {
      var closure = MinimalCover.Core.MinimalCover.ComputeClosure(testData.Attributes, testData.Fds);
      Assert.Equal(testData.Closure, closure);
    }

    [Theory]
    [MemberData(nameof(RemoveExtrasAttributesLhsTheoryData))]
    public void RemoveExtrasAttributesLhs_SimpleCall_MatchesExpectedSet(FdsTestData testData)
    {
      var fdsSet = MinimalCover.Core.MinimalCover.RemoveExtrasAttributesLhs(testData.Fds);
      Assert.Equal(testData.ExpectedFds, fdsSet);
    }

    [Fact]
    public void RemoveExtraFds_HaveNoSingleRhsFds_ThrowsArgumentException()
    {
      var sampleFds = new HashSet<FunctionalDependency> {
        new FunctionalDependency("A", "B,E"),
        new FunctionalDependency("C", "D"),
      };

      Assert.Throws<ArgumentException>(() => MinimalCover.Core.MinimalCover.RemoveExtraFds(sampleFds));
    }

    [Theory]
    [MemberData(nameof(RemoveExtraFdsTheoryData))]
    public void RemoveExtraFds_SimpleCall_MatchesExpectedSet(FdsTestData testData)
    {
      var fdsSet = MinimalCover.Core.MinimalCover.RemoveExtraFds(testData.Fds);
      Assert.Equal(testData.ExpectedFds, fdsSet);
    }

  }
}
