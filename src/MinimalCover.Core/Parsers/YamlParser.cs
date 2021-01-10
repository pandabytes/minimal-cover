using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace MinimalCover.Core.Parsers
{
  public static class YamlParser
  {
    public static ReadOnlySet<FunctionalDependency> Parse(string value)
    {
      var fdsSet = new HashSet<FunctionalDependency>();
      using (var reader = new StringReader(value.Trim()))
      {
        var yaml = new YamlStream();
        yaml.Load(reader);

        var rootNode = (YamlSequenceNode) yaml.Documents[0].RootNode;
        foreach (YamlMappingNode node in rootNode.Children)
        {
          var leftProperty = (YamlSequenceNode)node.Children[new YamlScalarNode("left")];
          var rightProperty = (YamlSequenceNode)node.Children[new YamlScalarNode("right")];

          var leftAttributes = leftProperty.Children.Select(n => ((YamlScalarNode)n).Value).ToHashSet();
          var rightAttributes = rightProperty.Children.Select(n => ((YamlScalarNode)n).Value).ToHashSet();
          
          var fd = new FunctionalDependency(leftAttributes, rightAttributes);
          fdsSet.Add(fd);
        }
      }

      return new ReadOnlySet<FunctionalDependency>(fdsSet);
    }
  }
}
