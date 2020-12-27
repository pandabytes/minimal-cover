using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;

namespace MinimalCover.Core.Parsers
{
  public static class JsonParser
  {
    /// <summary>
    /// Helper class used to convert JSON string to a set of
    /// <see cref="FunctionalDependency"/> objects. If schema
    /// is updated, then this class will need to be 
    /// updated as well
    /// </summary>
    public class FdSetConverter : JsonConverter
    {
      public override bool CanConvert(Type objectType) => objectType == typeof(ISet<FunctionalDependency>);

      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
      {
        var fdsSet = new HashSet<FunctionalDependency>();
        JArray jsonArray = JArray.Load(reader);

        foreach (var jsonObj in jsonArray)
        {
          var left = jsonObj["left"].ToObject<HashSet<string>>();
          var right = jsonObj["right"].ToObject<HashSet<string>>();
          var fd = new FunctionalDependency(left, right);
          fdsSet.Add(fd);
        }
        return fdsSet;
      }

      public override bool CanWrite => false;

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
        throw new NotImplementedException();
      }
    }

    public static readonly string SchemaFilePath = @".\Parsers\fd-schema.json";

    /// <summary>
    /// Parse the given <paramref name="value"/> into a set of
    /// <see cref="FunctionalDependency"/>
    /// </summary>
    /// <param name="value">The string value to parse</param>
    /// <returns>Set of parsed <see cref="FunctionalDependency"/></returns>
    public static ReadOnlySet<FunctionalDependency> Parse(string value)
    {
      using (var reader = new StreamReader(SchemaFilePath))
      {
        JArray jsonArray = (JArray)ValidateJson(value, reader.ReadToEnd());
        var converter = new FdSetConverter();
        var fdsSet = JsonConvert.DeserializeObject<ISet<FunctionalDependency>>(jsonArray.ToString(), converter);
        return new ReadOnlySet<FunctionalDependency>(fdsSet);
      }
    }

    private static JToken ValidateJson(string value, string schemaStr)
    {
      // Validate schema
      JSchema schema = JSchema.Parse(schemaStr);
      var jToken = JToken.Parse(value);

      // There may be many error validations, but only
      // need to throw one at a time
      IList<ValidationError> errors;
      _ = jToken.IsValid(schema, out errors);
      foreach (var i in errors)
      {
        string message = $"{i.Message} Path: {i.Path}. Line number: {i.LineNumber}";
        throw new ArgumentException(message);
      }

      return jToken;
    }

  }
}
