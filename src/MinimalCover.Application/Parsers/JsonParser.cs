using System;
using System.IO;
using System.Collections.Generic;

using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers.Settings;

namespace MinimalCover.Application.Parsers
{
  /// <summary>
  /// This class is responsible for parsing functional dependencies
  /// that are in <see cref="ParseFormat.Json"/>
  /// </summary>
  public abstract class JsonParser : IParser
  {
    /// <summary>
    /// The JSON schema that defines the structure of a 
    /// list of functional dependencies
    /// </summary>
    public string Schema { get; }

    /// <summary>
    /// Return <see cref="ParseFormat.Json"/>
    /// </summary>
    ParseFormat IParser.Format { get { return ParseFormat.Json; } }

    /// <summary>
    /// Interface method <see cref="IParser.Parse(string)"/>
    /// </summary>
    public abstract ISet<FunctionalDependency> Parse(string value);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Throw when <paramref name="settings"/> is null
    /// </exception>
    /// <param name="settings">JSON parser settings</param>
    public JsonParser(JsonParserSettings settings)
    {
      _ = settings?.SchemaFilePath ?? throw new ArgumentNullException($"Schema path must not be null");

      // Load in the schema file
      Schema = File.ReadAllText(settings.SchemaFilePath);
    }

    /// <summary>
    /// Validate the <paramref name="jsonStr"/> against the 
    /// <see cref="Schema"/>
    /// </summary>
    /// <remarks>
    /// The exact returned object is left up to whoever implements this method
    /// </remarks>
    /// <param name="jsonStr">JSON string to be validated</param>
    /// <exception cref="ParserException">
    /// Throws when validation fails
    /// </exception>
    /// <returns>The object parsed from <paramref name="jsonStr"/></returns>
    protected abstract object ValidateJson(string jsonStr);
    
  }
}