using Xunit;
using MinimalCover.Xunit.Core.Data;
using MinimalCover.Core;
namespace MinimalCover.Xunit.Core
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
