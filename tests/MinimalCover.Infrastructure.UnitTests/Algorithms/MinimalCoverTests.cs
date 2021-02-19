using System.Collections.Generic;

using MinimalCover.Application.Algorithms;
using MinimalCover.Domain.Models;

using Xunit;

namespace MinimalCover.Infrastructure.UnitTests.Algorithms
{
  /// <summary>
  /// This class encapsulates all the tests for any class
  /// that inhereit <see cref="IMinimalCover"/>. 
  /// 
  /// To test an implementation of <see cref="IMinimalCover"/>, simply
  /// inhereit <see cref="IMinimalCoverTests"/> and provide
  /// the implementation to the tests
  /// </summary>
  public abstract class IMinimalCoverTests
  {
    /// <summary>
    /// Object under test
    /// </summary>
    protected IMinimalCover m_minimalCover;

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.ManyRhsAttributesFdsTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public abstract void GetSingleRhsAttributeFds_MoreThanOneRhsAttribute_CountEqualOrGreater(ISet<FunctionalDependency> testFds);

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.ManyRhsAttributesFdsTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public abstract void GetSingleRhsAttributeFds_MoreThanOneRhsAttribute_ReturnSingleRhsAttrbSet(ISet<FunctionalDependency> testFds);

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.ManyRhsAttributesFdsTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public abstract void RemoveExtraFds_MoreThanOneRhsAttribute_ThrowsArgumentException(ISet<FunctionalDependency> testFds);

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.RemoveExtraLhsAttributesTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public abstract void RemoveExtrasLhsAttributes_FdsHaveExtraLhsAttrbs_ReturnsExpectedFds(MinimalCoverTestData.FdsTestData testData);

    [Theory]
    [MemberData(nameof(MinimalCoverTestData.RemoveExtraFdsTheoryData),
                MemberType = typeof(MinimalCoverTestData))]
    public abstract void RemoveExtraFds_ThereAreExtraFds_ReturnsExpectedFds(MinimalCoverTestData.FdsTestData testData);

  }
}
