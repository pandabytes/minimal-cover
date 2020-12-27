using Xunit;
using MinimalCover.Core.Xunit.Data;
using MinimalCover.Core;

namespace MinimalCover.Core.Xunit
{
  public class RelationTest 
  {
    [Theory]
    [MemberData(nameof(RelationTestData.TestData), 
                MemberType = typeof(RelationTestData))]
    public void Constructor_Test(Relation expectedRelation)
    {
      var relation = new Relation(expectedRelation.Attributes, expectedRelation.Fds);
      Assert.True(relation.Fds.SetEquals(expectedRelation.Fds));
      Assert.True(relation.Attributes.SetEquals(expectedRelation.Attributes));
    }


  }
}
