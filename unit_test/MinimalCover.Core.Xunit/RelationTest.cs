using Xunit;
using MinimalCover.Core.Xunit.Data;

namespace MinimalCover.Core.Xunit
{
  public class RelationTests
  {
    [Theory]
    [MemberData(nameof(RelationTestData.TestData), 
                MemberType = typeof(RelationTestData))]
    public void Constructor_WithAttributesAndFds_AllPopulated(Relation expectedRelation)
    {
      var relation = new Relation(expectedRelation.Attributes, expectedRelation.Fds);
      Assert.True(relation.Fds.SetEquals(expectedRelation.Fds));
      Assert.True(relation.Attributes.SetEquals(expectedRelation.Attributes));
    }

  }
}
