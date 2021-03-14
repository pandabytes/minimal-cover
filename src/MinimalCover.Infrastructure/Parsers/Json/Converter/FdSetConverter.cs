using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using MinimalCover.Domain.Models;

namespace MinimalCover.Infrastructure.Parsers.Json.Converter
{
  /// <summary>
  /// Helper class used to convert JSON string to a set of
  /// <see cref="FunctionalDependency"/> objects.
  /// </summary>
  internal class FdSetConverter : JsonConverter
  { 
    /// <summary>
    /// Determines whether this instance can convert the 
    /// specified object type to <see cref="IEnumerable{FunctionalDependency}"/>
    /// </summary>
    /// <remarks>
    /// If extending this class, it's typically not needed to override this method
    /// unless you want the converter to return a different type
    /// </remarks>
    /// <param name="objectType">Object type to check if it can be converted</param>
    /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
    public override bool CanConvert(Type objectType) => typeof(IEnumerable<FunctionalDependency>).IsSubclassOf(objectType);

    /// <summary>
    /// Read the JSON content and convert it to <see cref="IEnumerable{FunctionalDependency}"/> object
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="objectType"></param>
    /// <param name="existingValue"></param>
    /// <param name="serializer"></param>
    /// <returns>Return <see cref="IEnumerable{FunctionalDependency}"/> object</returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      JArray jsonArray = JArray.Load(reader);

      var fdsArray = jsonArray.Select(jsonObj =>
      {
        var left = jsonObj["left"].ToObject<HashSet<string>>();
        var right = jsonObj["right"].ToObject<HashSet<string>>();
        return new FunctionalDependency(left, right);
      });

      return fdsArray;
    }      

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }
  }
}
