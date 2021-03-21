using System;
using System.IO;
using System.CommandLine;
using System.CommandLine.Invocation;

using MinimalCover.Application;
using MinimalCover.Application.Algorithms;
using MinimalCover.Application.Parsers;
using MinimalCover.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace MinimalCover.UI.Console
{
  public class Program
  {
    /// <summary>
    /// Main entry of the console app
    /// </summary>
    /// <param name="args">Arguments</param>
    public static int Main(string[] args)
    {
      // Config
      var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
      
      // Register the services
      var services = new ServiceCollection();
      services.AddParsers(config)
              .AddMinimalCoverAlgs();
      var provider = services.BuildServiceProvider();

      // Create arg parser
      var rootCommand = new RootCommand
      {
        new Option<ParseFormat>(
          new string[2] { "-i", "--input" },
          description: "The input format of functional dependencies"),
        new Option<bool>(
          new string[2] { "-f", "--file" },
          description: "Specify whether the functional dependency argument is a file"),
        new Argument("fds")
      };
      rootCommand.Description = "Find the minimal cover given a list of functional dependencies";

      rootCommand.Handler = CommandHandler.Create<ParseFormat, bool, string>((input, file, fds) =>
      {
        // Load the content of the file if the "value" is a file path
        string value = (file) ? File.ReadAllText(fds) : fds;

        // Get the parser based on the input format
        IParser parser = GetParser(provider, input);
        
        var mc = provider.GetRequiredService<IMinimalCover>();
        var app = new MinimalCoverApp(mc);
        var result = app.FindMinimalCover(value, parser);

        // Display minimal cover
        System.Console.WriteLine($"Minimal Cover ({result.Count})");
        foreach (var fd in result)
        {
          System.Console.WriteLine(fd);
        }
      });
      
      return rootCommand.InvokeAsync(args).Result;
    }

    public static IParser GetParser(ServiceProvider provider, ParseFormat format)
    {
      // Get the parser based on the input format
      switch (format)
      {
        case ParseFormat.Text:
          return provider.GetRequiredService<TextParser>();
        case ParseFormat.Json:
          return provider.GetRequiredService<JsonParser>();
        default:
          throw new NotSupportedException($"Format \"{format}\" is not supported yet");
      }
    }
  }
}
