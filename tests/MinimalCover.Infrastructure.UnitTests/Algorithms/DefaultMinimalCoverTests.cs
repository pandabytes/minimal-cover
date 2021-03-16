using System;
using System.Collections.Generic;
using System.Linq;

using MinimalCover.Application.Algorithms;
using MinimalCover.Domain.Models;
using static MinimalCover.Infrastructure.UnitTests.ConfigurationUtils;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace MinimalCover.Infrastructure.UnitTests.Algorithms
{
  public class DefaultMinimalCoverTests : MinimalCoverTests
  {
    /// <summary>
    /// Constructor
    /// </summary>
    public DefaultMinimalCoverTests()
    {
      // Pass empty config since MinimalCover doesn't require any configuration
      var dp = new DependencyInjection(EmptyConfiguration);
      m_minimalCover = dp.Provider.GetRequiredService<IMinimalCover>();
    }

    public override void GetSingleRhsAttributeFds_MoreThanOneRhsAttribute_CountEqualOrGreater(ISet<FunctionalDependency> testFds)
    {
      var fds = m_minimalCover.GetSingleRhsAttributeFds(testFds);

      // If the testFds have no fds that have more than 1 RHS attributes
      // the count of fds should be the same, else it should be greater than
      // testFds's count
      Assert.True(fds.Count >= testFds.Count,
                  $"Size of returned set is {fds.Count} but it must be at least {testFds.Count}");
    }

    public override void GetSingleRhsAttributeFds_MoreThanOneRhsAttribute_ReturnSingleRhsAttrbSet(ISet<FunctionalDependency> testFds)
    {
      var fds = m_minimalCover.GetSingleRhsAttributeFds(testFds);
      var allHaveSingleRhsAttrbs = fds.All(fd => fd.Right.Count == 1);
      Assert.True(allHaveSingleRhsAttrbs, "Not all functional dependencies have single RHS attribute");
    }

    public override void RemoveExtraFds_MoreThanOneRhsAttribute_ThrowsArgumentException(ISet<FunctionalDependency> testFds)
    {
      Assert.Throws<ArgumentException>(() => m_minimalCover.RemoveExtraFds(testFds));
    }

    public override void RemoveExtrasLhsAttributes_FdsHaveExtraLhsAttrbs_ReturnsExpectedFds(MinimalCoverTestData.FdsTestData testData)
    {
      var actualFds = m_minimalCover.RemoveExtrasLhsAttributes(testData.InputFds);
      Assert.Equal(testData.ExpectedFds, actualFds);
    }

    public override void RemoveExtraFds_ThereAreExtraFds_ReturnsExpectedFds(MinimalCoverTestData.FdsTestData testData)
    {
      var actualFds = m_minimalCover.RemoveExtraFds(testData.InputFds);
      Assert.Equal(testData.ExpectedFds, actualFds);
    }

  }
}
