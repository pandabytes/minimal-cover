using System;
using System.Collections.Generic;
using System.Linq;

using MinimalCover.Application.Algorithms;
using MinimalCover.Domain.Models;
using MinimalCover.Infrastructure.Algorithms;
using Xunit;

namespace MinimalCover.Infrastructure.UnitTests.Algorithms
{
  public class DefaultMinimalCoverTests
  {
    private readonly IMinimalCover m_defaultMinimalCover;

    public DefaultMinimalCoverTests()
    {
      m_defaultMinimalCover = new DefaultMinimalCover();
    }

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.ManyRhsAttributesFdsTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public void GetSingleRhsAttributeFds_MoreThanOneRhsAttribute_CountEqualOrGreater(ISet<FunctionalDependency> testFds)
    {
      var fds = m_defaultMinimalCover.GetSingleRhsAttributeFds(testFds);

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
      var fds = m_defaultMinimalCover.GetSingleRhsAttributeFds(testFds);
      var allHaveSingleRhsAttrbs = fds.All(fd => fd.Right.Count == 1);
      Assert.True(allHaveSingleRhsAttrbs, "Not all functional dependcies have single RHS attribute");
    }

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.ManyRhsAttributesFdsTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public void RemoveExtraFds_MoreThanOneRhsAttribute_ThrowsArgumentException(ISet<FunctionalDependency> testFds)
    {
      Assert.Throws<ArgumentException>(() => m_defaultMinimalCover.RemoveExtraFds(testFds));
    }

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.RemoveExtraLhsAttributesTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public void RemoveExtrasLhsAttributes_FdsHaveExtraLhsAttrbs_ReturnsExpectedFds(MinimalCoverTestData.FdsTestData testData)
    {
      var actualFds = m_defaultMinimalCover.RemoveExtrasLhsAttributes(testData.InputFds);
      Assert.Equal(testData.ExpectedFds, actualFds);
    }

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.RemoveExtraFdsTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public void RemoveExtraFds_ThereAreExtraFds_ReturnsExpectedFds(MinimalCoverTestData.FdsTestData testData)
    {
      var actualFds = m_defaultMinimalCover.RemoveExtraFds(testData.InputFds);
      Assert.Equal(testData.ExpectedFds, actualFds);
    }

  }
}
