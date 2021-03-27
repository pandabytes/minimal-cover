﻿using System;
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
      catch (Exception ex)
      {
        throw new ParserException("JSON parser wasn't able to parse correctly. " +
                                  "Schema file may have been modified without updating " +
                                  "the JSON parser implementation", ex);
      }
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