using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace MinimalCover.Core.Parsers
{
  public static class YamlParser
  {
    public static readonly string MissingPropertiesMessage = "A functional depedency must have these properties: left, right";
    public static readonly string EmptyLhsOrRhsMessage = "Property left and right must not be empty";
    public static readonly string NonYamlListMessage = "Left and right property must a list";

    /// <summary>
    /// Parse the given <paramref name="value"/> into a set of
    /// <see cref="FunctionalDependency"/>
    /// </summary>
    /// <param name="value">The string value to parse</param>
    /// <returns>Set of parsed <see cref="FunctionalDependency"/></returns>
    public static ReadOnlySet<FunctionalDependency> Parse(string value)
    {
      var fdsSet = new HashSet<FunctionalDependency>();
      using (var reader = new StringReader(value.Trim()))
      {
        var yaml = new YamlStream();
        yaml.Load(reader);

        const string ErrorMessageFormat = "{0}. Please check functional dependency {1}";
        var rootNode = (YamlSequenceNode)yaml.Documents[0].RootNode;
        
        for (int i = 0; i < rootNode.Children.Count; i++)
        {
          var fdNode = (YamlMappingNode)rootNode.Children[i];
          var children = fdNode.Children;

          var leftNode = new YamlScalarNode("left");
          var rightNode = new YamlScalarNode("right");

          // Check if a yaml item contains both "left" and "right"
          if (!(children.ContainsKey(leftNode) &&
                children.ContainsKey(rightNode)))
          {
            var errMessage = string.Format(ErrorMessageFormat, MissingPropertiesMessage, i + 1);
            throw new ArgumentException(errMessage);
          }

          var leftPropertyValue = children[leftNode] as YamlSequenceNode;
          var rightPropertyValue = children[rightNode] as YamlSequenceNode;

          // "left" and "right" must be yaml list
          if (leftPropertyValue == null || rightPropertyValue == null)
          {
            var errMessage = string.Format(ErrorMessageFormat, NonYamlListMessage, i + 1);
            throw new ArgumentException(errMessage);
          }

          var leftAttributes = leftPropertyValue.Children.Select(n => ((YamlScalarNode)n).Value).ToHashSet();
          var rightAttributes = rightPropertyValue.Children.Select(n => ((YamlScalarNode)n).Value).ToHashSet();

          // Attributes must not be empty
          if (leftAttributes.Count < 1 || rightAttributes.Count < 1)
          {
            var errMessage = string.Format(ErrorMessageFormat, EmptyLhsOrRhsMessage, i + 1);
            throw new ArgumentException(errMessage);
          }

          var fd = new FunctionalDependency(leftAttributes, rightAttributes);
          fdsSet.Add(fd);
        }

        return new ReadOnlySet<FunctionalDependency>(fdsSet);
      }

    }

  }
}
