using System;
using System.Collections.Generic;
using MinimalCover.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MinimalCover.Core.Parsers.Json
{
  public class FunctionalDependencyConverter : JsonConverter
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
}
