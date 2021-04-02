using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;

using MinimalCover.Domain.Core;
using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Parsers.Settings;

namespace MinimalCover.Infrastructure.Parsers.Json.Converter
{
  /// <summary>
  /// Default implementation of <see cref="JsonParser"/>.
  /// </summary>
  internal class JsonConverterParser : JsonParser
  {
    /// <summary>
    /// Constructor
    /// </summary>
    public JsonConverterParser(JsonParserSettings settings)
      : base(settings)
    {}

    /// <inheritdoc/>
    public override ISet<FunctionalDependency> Parse(string value)
    {
      JArray jsonArray = (JArray)ValidateJson(value);

      try
      {
        // Null forgiving here because we already validate the input JSON string above
        var fds = JsonConvert.DeserializeObject<IEnumerable<FunctionalDependency>>(jsonArray.ToString())!;
        return new ReadOnlySet<FunctionalDependency>(fds.ToHashSet());
      }
      // Occur when Newtonsoft fails to serialize the JSON string to our FunctionalDependency class
      catch (JsonSerializationException ex)
      {
        throw new ParserException("Unable to parse string value to a set of functional dependencies", ex);
      }
      catch (ArgumentException ex)
      {
        throw new ParserException("Unable to construct functional dependency objects", ex);
      }
    }

    /// <inheritdoc/>
    /// <exception cref="ParserException">
    /// Throws when <paramref name="value"/> doesn't match with 
    /// the schema defined in <see cref="Schema"/>. Or thrown
    /// when the <paramref name="jsonStr"/> has syntax errors
    /// </exception>
    /// <returns>The <see cref="JArray"/> object</returns>
    protected override object ValidateJson(string jsonStr)
    {
      // Parse the schema
      JSchema schema = JSchema.Parse(Schema);
      JToken jToken;
      try
      {
        jToken = JToken.Parse(jsonStr);
      }
      catch (JsonReaderException ex)
      {
        throw new ParserException($"Fail to parse the given JSON string \"{jsonStr}\". " + 
                                   "This string may not be in correct JSON format", ex);
      }

      // Throw exception if validation fails and include all the 
      // failed validation in the exception message
      _ = jToken.IsValid(schema, out IList<ValidationError> errors);
      if (errors.Count > 0)
      {
        var message = $"Fail to validate JSON string.";
        foreach (var error in errors)
        {
          message += $"{Environment.NewLine}{error.Message} Path: {error.Path}. Line number: {error.LineNumber}.";
        }
        throw new ParserException(message);
      }

      return jToken;
    }

  }
}