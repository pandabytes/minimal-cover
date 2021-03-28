using System;
using System.Collections.Generic;

using MinimalCover.Application.Algorithms;
using MinimalCover.Domain.Models;
using MinimalCover.UnitTests.Utils;
using Xunit;

namespace MinimalCover.Application.UnitTests.Algorithms
{
  /// <summary>
  /// This class contains sample test data to test 
  /// the implementation of <see cref="IMinimalCover"/>
  /// </summary>
  public static class MinimalCoverTestData
  {
    /// <summary>
    /// This class is used to specify the <see cref="InputFds"/>
    /// and the <see cref="ExpectedFds"/>. These properties
    /// will be used in the tests to verify whether the actual
    /// set of functional dependencies, given <see cref="InputFds"/>,
    /// matches with <see cref="ExpectedFds"/>
    /// </summary>
    public class FdsTestData
    {
      public ISet<FunctionalDependency> InputFds { get; set; } = null!;

      public ISet<FunctionalDependency> ExpectedFds { get; set; } = null!;
    }

    /// <summary>
    /// Test data containing functional dependencies that have
    /// more than 1 attribute on RHS
    /// </summary>
    public readonly static TheoryData<ISet<FunctionalDependency>> ManyRhsAttributesFdsTheoryData =
      new TheoryData<ISet<FunctionalDependency>>()
      {
        new HashSet<FunctionalDependency>()
        {
          FuncDepUtils.ConstructFdFromString("A", "B,J,K"),
        },

        new HashSet<FunctionalDependency>()
        {
          FuncDepUtils.ConstructFdFromString("A", "B"),
          FuncDepUtils.ConstructFdFromString("A", "B,C")
        },

        new HashSet<FunctionalDependency>()
        {
          FuncDepUtils.ConstructFdFromString("A", "B"),
          FuncDepUtils.ConstructFdFromString("A", "B,C"),
          FuncDepUtils.ConstructFdFromString("A,D", "B,C")
        }
      };

    /// <summary>
    /// Test data for the
    /// method <see cref="IMinimalCover.RemoveExtrasLhsAttributes(ISet{FunctionalDependency})"/>
    /// </summary>
    public readonly static TheoryData<FdsTestData> RemoveExtraLhsAttributesTheoryData =
      new TheoryData<FdsTestData>()
      {
        new FdsTestData()
        {
          InputFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A,B", "C"),
            FuncDepUtils.ConstructFdFromString("C", "A"),
            FuncDepUtils.ConstructFdFromString("B,C", "D"),
            FuncDepUtils.ConstructFdFromString("A,C,D", "B"),
            FuncDepUtils.ConstructFdFromString("A,C,D", "D"),
            FuncDepUtils.ConstructFdFromString("D", "E"),
            FuncDepUtils.ConstructFdFromString("D", "G"),
            FuncDepUtils.ConstructFdFromString("B,E", "C"),
            FuncDepUtils.ConstructFdFromString("C,G", "B"),
            FuncDepUtils.ConstructFdFromString("C,G", "D"),
            FuncDepUtils.ConstructFdFromString("C,E", "A"),
            FuncDepUtils.ConstructFdFromString("C,E", "G")
          },
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A,B", "C"),
            FuncDepUtils.ConstructFdFromString("C", "A"),
            FuncDepUtils.ConstructFdFromString("B,C", "D"),
            FuncDepUtils.ConstructFdFromString("C,D", "B"),
            FuncDepUtils.ConstructFdFromString("C,D", "D"),
            FuncDepUtils.ConstructFdFromString("D", "E"),
            FuncDepUtils.ConstructFdFromString("D", "G"),
            FuncDepUtils.ConstructFdFromString("B,E", "C"),
            FuncDepUtils.ConstructFdFromString("C,G", "B"),
            FuncDepUtils.ConstructFdFromString("C,G", "D"),
            FuncDepUtils.ConstructFdFromString("C,E", "A"),
            FuncDepUtils.ConstructFdFromString("C,E", "G")
          }
        },

        new FdsTestData()
        {
          InputFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A", "D"),
            FuncDepUtils.ConstructFdFromString("B,C", "D"),
            FuncDepUtils.ConstructFdFromString("B,C", "A"),
            FuncDepUtils.ConstructFdFromString("C", "B"),
            FuncDepUtils.ConstructFdFromString("E", "D"),
            FuncDepUtils.ConstructFdFromString("E", "A")
          },
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A", "D"),
            FuncDepUtils.ConstructFdFromString("C", "D"),
            FuncDepUtils.ConstructFdFromString("C", "A"),
            FuncDepUtils.ConstructFdFromString("C", "B"),
            FuncDepUtils.ConstructFdFromString("E", "D"),
            FuncDepUtils.ConstructFdFromString("E", "A")
          }
        }
      };

    /// <summary>
    /// Test data for the
    /// method <see cref="IMinimalCover.RemoveExtraFds(ISet{FunctionalDependency})"/>
    /// </summary>
    public static TheoryData<FdsTestData> RemoveExtraFdsTheoryData =
      new TheoryData<FdsTestData>()
      {
        new FdsTestData()
        {
          InputFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A", "D"),
            FuncDepUtils.ConstructFdFromString("C", "D"),
            FuncDepUtils.ConstructFdFromString("C", "A"),
            FuncDepUtils.ConstructFdFromString("C", "B"),
            FuncDepUtils.ConstructFdFromString("E", "D"),
            FuncDepUtils.ConstructFdFromString("E", "A")
          },
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A", "D"),
            FuncDepUtils.ConstructFdFromString("C", "A"),
            FuncDepUtils.ConstructFdFromString("C", "A"),
            FuncDepUtils.ConstructFdFromString("C", "B"),
            FuncDepUtils.ConstructFdFromString("E", "A")
          }
        },

        new FdsTestData()
        {
          InputFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A,B", "C"),
            FuncDepUtils.ConstructFdFromString("C", "A"),
            FuncDepUtils.ConstructFdFromString("B,C", "D"),
            FuncDepUtils.ConstructFdFromString("C,D", "B"),
            FuncDepUtils.ConstructFdFromString("C,D", "D"),
            FuncDepUtils.ConstructFdFromString("D", "E"),
            FuncDepUtils.ConstructFdFromString("D", "G"),
            FuncDepUtils.ConstructFdFromString("B,E", "C"),
            FuncDepUtils.ConstructFdFromString("C,G", "B"),
            FuncDepUtils.ConstructFdFromString("C,G", "D"),
            FuncDepUtils.ConstructFdFromString("C,E", "A"),
            FuncDepUtils.ConstructFdFromString("C,E", "G")
          },
          ExpectedFds = new HashSet<FunctionalDependency>()
          {
            FuncDepUtils.ConstructFdFromString("A,B", "C"),
            FuncDepUtils.ConstructFdFromString("C", "A"),
            FuncDepUtils.ConstructFdFromString("B,C", "D"),
            FuncDepUtils.ConstructFdFromString("D", "E"),
            FuncDepUtils.ConstructFdFromString("D", "G"),
            FuncDepUtils.ConstructFdFromString("B,E", "C"),
            FuncDepUtils.ConstructFdFromString("C,G", "B"),
            FuncDepUtils.ConstructFdFromString("C,E", "G")
          }
        }
      };

  }
}
