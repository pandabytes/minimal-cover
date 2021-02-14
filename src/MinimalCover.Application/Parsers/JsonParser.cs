using System;
using System.Collections.Generic;

using MinimalCover.Domain.Models;

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
    public string Schema { get; protected set; }

    /// <summary>
    /// Return <see cref="ParseFormat.Json"/>
    /// </summary>
    ParseFormat IParser.Format { get { return ParseFormat.Json; } }

    /// <summary>
    /// Interface method <see cref="IParser.Parse(string)"/>
    /// </summary>
    public abstract ISet<FunctionalDependency> Parse(string value);

    /// <summary>
    /// Create a JSON parser that uses <paramref name="schema"/>
    /// to validate during parsing
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="schema"/> is null or empty
    /// </exception>
    /// <param name="schema"></param>
    public JsonParser(string schema)
    {
      if (string.IsNullOrWhiteSpace(schema))
      {
        throw new ArgumentException($"{nameof(schema)} cannot be null or empty");
      }
      Schema = schema;
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    protected JsonParser() { }

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