using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using MinimalCover.Core.Parsers;

namespace MinimalCover.Console
{
  public class Program
  {
    public enum InputType
    {
      // Command line interface
      Cli,
      Text,
      Json
    }

    public delegate void testc(int x);

    /// <summary>
    ///  --input=text "..\..\..\TestData\fds_3.txt"
    ///  --input=cli "A-->B;C-->D;A-->D"
    ///  -i json "..\..\..\TestData\fds_1.json"
    /// </summary>
    /// <param name="args"></param>
    public static int Main(string[] args)
    {
      var rootCommand = new RootCommand
      {
        new Option<InputType>(
          new string[2] { "-i", "--input" },
          description: "The input type of functional dependencies"),
        new Argument("fds")
      };
      rootCommand.Description = "Find the minimal cover given a list of functional dependencies";

      rootCommand.Handler = CommandHandler.Create<InputType, string>((input, fds) =>
      {
        // Parse the functional dependencies
        IParser parser = null;
        switch (input)
        {
          case InputType.Cli:
            parser = new Core.Parsers.Cli.CliParser(fds);
            break;
          case InputType.Text:
            parser = new Core.Parsers.Text.TextFileParser(fds);
            break;
          case InputType.Json:
            parser = new Core.Parsers.Json.JsonFileParser(fds);
            break;
          default:
            // System.CommandLine should handle the list of valid input types
            throw new NotSupportedException("Default parser is not supported");
        }
        parser.Parse();

        // 1. Single attribute RHS
        var fdsSet = Core.MinimalCover.GetSingleAttributeRhsFds(parser.ParsedFds);
        System.Console.WriteLine("\n1. Make all fds have single attribute on RHS");
        foreach (var fd in fdsSet)
        {
          System.Console.WriteLine(fd);
        }

        // 2. Remove extranenous attributes on LHS
        var noExtra = Core.MinimalCover.RemoveExtrasAttributesLhs(fdsSet);
        System.Console.WriteLine("\n2. Remove extraneous attributes on LHS");
        foreach (var fd in noExtra)
        {
          System.Console.WriteLine(fd);
        }

        // 3. Remove extra fds
        var minimalCover = Core.MinimalCover.RemoveExtraFds(noExtra);
        System.Console.WriteLine($"\n3. Remove extranenous fds");
        System.Console.WriteLine($"Minimal Cover ({minimalCover.Count})");
        foreach (var fd in minimalCover)
        {
          System.Console.WriteLine(fd);
        }


      });

      return rootCommand.InvokeAsync(args).Result;
    }
  }
}
