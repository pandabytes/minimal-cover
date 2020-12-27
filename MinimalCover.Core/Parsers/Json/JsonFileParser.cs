using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;

namespace MinimalCover.Core.Parsers.Json
{
  public class JsonFileParser : FileParser
  {
    protected const string SchemaFilePath = @".\Parsers\Json\fd-schema.json";

    public JsonFileParser(string filePath) : base(filePath) { }

    public override void Parse()
    {
      if (ParsedFds.Count > 0)
      {
        throw new InvalidOperationException($"{FilePath} is already parsed");
      }

      JArray jsonArray = (JArray)ValidateJsonFile(FilePath, SchemaFilePath);
      var converter = new FunctionalDependencyConverter();
      var fdsSet = JsonConvert.DeserializeObject<ISet<FunctionalDependency>>(jsonArray.ToString(), converter);
      m_parsedFds.UnionWith(fdsSet);
    }

    protected static JToken ValidateJsonFile(string filePath, string schemaFilePath)
    {
      // Validate schema
      using (var schemaReader = File.OpenText(schemaFilePath))
      using (var fileReader = File.OpenText(filePath))
      {
        JSchema schema = JSchema.Parse(schemaReader.ReadToEnd());
        var jToken = JToken.Parse(fileReader.ReadToEnd());

        // There may be many error validations, but only
        // need to throw one at a time
        IList<ValidationError> errors;
        bool valid = jToken.IsValid(schema, out errors);
        foreach (var i in errors)
        {
          string message = $"{i.Message} Path: {i.Path}";
          throw new FileParserException(i.LineNumber, message);
        }

        return jToken;
      }
    }

  }
}
