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
  /// This implementation relies on a converter object to
  /// convert the JSON string to a set of functional dependencies
  /// </summary>
  internal class JsonConverterParser : JsonParser
  {
    /// <summary>
    /// Converter object that is used to convert JSON string
    /// to a collection of <see cref="FunctionalDependency"/>
    /// </summary>
    private readonly FdSetConverter m_converter;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <remarks>
    /// The <paramref name="converter"/> must match with the schema file
    /// specified in <paramref name="settings"/>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="converter"/> is null
    /// </exception>
    /// <param name="converter">Converter object</param>
    public JsonConverterParser(JsonParserSettings settings, FdSetConverter converter)
      : base(settings)
    {
      _ = converter ?? throw new ArgumentNullException(nameof(converter));
      m_converter = converter;
    }

    /// <inheritdoc/>
    public override ISet<FunctionalDependency> Parse(string value)
    {
      JArray jsonArray = (JArray)ValidateJson(value); 
      var fds = JsonConvert.DeserializeObject<IEnumerable<FunctionalDependency>>(jsonArray.ToString(), m_converter);
      return new ReadOnlySet<FunctionalDependency>(fds.ToHashSet());
    }

    /// <inheritdoc/>
    /// <exception cref="ParserException">
    /// Throws when <paramref name="value"/> doesn't match with 
    /// the schema defined in <see cref="Schema"/>
    /// </exception>
    /// <returns>The <see cref="JArray"/> object</returns>
    protected override object ValidateJson(string jsonStr)
    {
      // Parse the schema
      JSchema schema = JSchema.Parse(Schema);
      var jToken = JToken.Parse(jsonStr);

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