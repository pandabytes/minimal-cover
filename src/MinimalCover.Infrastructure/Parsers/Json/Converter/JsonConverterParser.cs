using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;

using MinimalCover.Domain.Core;
using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers;

namespace MinimalCover.Infrastructure.Parsers.Json.Converter
{
  /// <summary>
  /// Default implementation of <see cref="JsonParser"/>.
  /// This implementation relies on a converter object to
  /// convert the JSON string to a set of functional dependencies
  /// </summary>
  public class JsonConverterParser : JsonParser
  {
    /// <summary>
    /// Object that contains the path to the
    /// schema file and provides method to 
    /// convert a given JSON string to a <see cref="IEnumerable{FunctionalDependency}"/>
    /// object, based on the defined schema
    /// </summary>
    private readonly FdSetConverter m_converter;

    /// <summary>
    /// Constructor that takes in a <see cref="FdSetConverter"/> object.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="converter"/> is null
    /// </exception>
    /// <param name="converter">Converter object</param>
    public JsonConverterParser(FdSetConverter converter)
    {
      _ = converter ?? throw new ArgumentNullException(nameof(converter));

      // Load in the schema file
      using (var reader = new StreamReader(converter.SchemaFilePath))
      {
        Schema = reader.ReadToEnd();
      }

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