using System;
using System.Collections.Generic;
using System.Linq;

using MinimalCover.Application.Algorithms;
using MinimalCover.Domain.Models;
using MinimalCover.UnitTests.Utils;
using static MinimalCover.UnitTests.Utils.ConfigurationUtils;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace MinimalCover.Application.UnitTests.Algorithms
{
  /// <summary>
  /// This class encapsulates all the tests for any class
  /// that inhereit <see cref="IMinimalCover"/>. 
  /// 
  /// To test an implementation of <see cref="IMinimalCover"/>, simply
  /// inhereit <see cref="MinimalCoverTests"/> and provide
  /// the implementation to the tests
  /// </summary>
  public class MinimalCoverTests
  {
    /// <summary>
    /// Object under test
    /// </summary>
    protected IMinimalCover m_minimalCover;

    /// <summary>
    /// Constructor
    /// </summary>
    public MinimalCoverTests()
    {
      // Pass empty config since MinimalCover doesn't require any configuration
      var dp = new DependencyInjection(EmptyConfiguration);
      m_minimalCover = dp.Provider.GetRequiredService<IMinimalCover>();
    }

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.ManyRhsAttributesFdsTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public void GetSingleRhsAttributeFds_MoreThanOneRhsAttribute_CountEqualOrGreater(ISet<FunctionalDependency> testFds)
    {
      var fds = m_minimalCover.GetSingleRhsAttributeFds(testFds);

      // If the testFds have no fds that have more than 1 RHS attributes
      // the count of fds should be the same, else it should be greater than
      // testFds's count
      Assert.True(fds.Count >= testFds.Count,
                  $"Size of returned set is {fds.Count} but it must be at least {testFds.Count}");
    }

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.ManyRhsAttributesFdsTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public void GetSingleRhsAttributeFds_MoreThanOneRhsAttribute_ReturnSingleRhsAttrbSet(ISet<FunctionalDependency> testFds)
    {
      var fds = m_minimalCover.GetSingleRhsAttributeFds(testFds);
      var allHaveSingleRhsAttrbs = fds.All(fd => fd.Right.Count == 1);
      Assert.True(allHaveSingleRhsAttrbs, "Not all functional dependencies have single RHS attribute");
    }

    [Fact]
    public void GetSingleRhsAttributeFds_EmptyArgument_ReturnsEmpty()
    {
      var emptySet = new HashSet<FunctionalDependency>();
      var fds = m_minimalCover.GetSingleRhsAttributeFds(emptySet);
      Assert.Empty(fds);
    }

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.ManyRhsAttributesFdsTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public void RemoveExtraFds_MoreThanOneRhsAttribute_ThrowsArgumentException(ISet<FunctionalDependency> testFds)
    {
      Assert.Throws<ArgumentException>(() => m_minimalCover.RemoveExtraFds(testFds));
    }

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.RemoveExtraLhsAttributesTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public void RemoveExtrasLhsAttributes_FdsHaveExtraLhsAttrbs_ReturnsExpectedFds(MinimalCoverTestData.FdsTestData testData)
    {
      var actualFds = m_minimalCover.RemoveExtrasLhsAttributes(testData.InputFds);
      Assert.Equal(testData.ExpectedFds, actualFds);
    }

    [Fact]
    public void RemoveExtrasLhsAttributes_EmptyArgument_ReturnsEmpty()
    {
      var emptySet = new HashSet<FunctionalDependency>();
      var fds = m_minimalCover.RemoveExtrasLhsAttributes(emptySet);
      Assert.Empty(fds);
    }

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.RemoveExtraFdsTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public void RemoveExtraFds_ThereAreExtraFds_ReturnsExpectedFds(MinimalCoverTestData.FdsTestData testData)
    {
      var actualFds = m_minimalCover.RemoveExtraFds(testData.InputFds);
      Assert.Equal(testData.ExpectedFds, actualFds);
    }

    [Fact]
    public void RemoveExtrasFds_EmptyArgument_ReturnsEmpty()
    {
      var emptySet = new HashSet<FunctionalDependency>();
      var fds = m_minimalCover.RemoveExtraFds(emptySet);
      Assert.Empty(fds);
    }
  }
}
