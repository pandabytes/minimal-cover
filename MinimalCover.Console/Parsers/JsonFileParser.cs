using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using MinimalCover.Core;

namespace MinimalCover.Console.Parsers
{
  public class JsonFileParser : FileParser
  {
    protected const string SchemaFilePath = @"..\..\..\TestData\fd-schema.json";

    public JsonFileParser(string filePath) : base(filePath) { }

    public override void Parse()
    {
      if (ParsedFds.Count > 0)
      {
        throw new InvalidOperationException($"{FilePath} is already parsed");
      }
      
      // @TODO: Create FD JSON friendly class
      JObject jsonObj = ValidateJsonFile(FilePath, SchemaFilePath);
      var fdsJsonObject = jsonObj.SelectToken("fds");
      foreach (var fdJsonObj in fdsJsonObject)
      {
        // There should only be 2 properties
        var left = fdJsonObj.First;
        var right = fdJsonObj.Last;
        var leftAttributes = left.First.ToObject<HashSet<string>>();
        var rightAttributes = right.First.ToObject<HashSet<string>>();
        var fd = new FunctionalDependency(leftAttributes, rightAttributes);
        m_parsedFds.Add(fd);
      }
    }

    protected static JObject ValidateJsonFile(string filePath, string schemaFilePath)
    {
      // Validate schema
      using (var schemaReader = File.OpenText(schemaFilePath))
      using (var fileReader = File.OpenText(filePath))
      {
        JSchema schema = JSchema.Parse(schemaReader.ReadToEnd());
        JObject jsonObj = JObject.Parse(fileReader.ReadToEnd());
        
        // There may be many validations, but only need to
        // throw one at a time
        IList<ValidationError> errors;
        bool valid = jsonObj.IsValid(schema, out errors);
        foreach (var i in errors)
        {
          string message = $"{i.Message} Path: {i.Path}";
          throw new FileParserException(i.LineNumber, message);
        }

        return jsonObj;
      }
    }

  }
}
