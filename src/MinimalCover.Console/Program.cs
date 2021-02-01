using System;
using System.IO;
using System.CommandLine;
using System.CommandLine.Invocation;
using MinimalCover.Core;
using MinimalCover.Core.Parsers;

namespace MinimalCover.Console
{
  public class Program
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static int Main(string[] args)
    {
      var rootCommand = new RootCommand
      {
        new Option<InputFormat>(
          new string[2] { "-i", "--input" },
          description: "The input format of functional dependencies"),
        new Option<bool>(
          new string[2] { "-f", "--file" },
          description: "Specify whether the functional dependency argument is a file"),
        new Argument("fds")
      };
      rootCommand.Description = "Find the minimal cover given a list of functional dependencies";

      rootCommand.Handler = CommandHandler.Create<InputFormat, bool, string>((input, file, fds) =>
      {
        // Parse the argument 
        string value = fds;
        if (file)
        {
          using (var streamReader = new StreamReader(fds))
          {
            value = streamReader.ReadToEnd();
          }
        }
        var parsedFds = Parser.Parse(input, value);

        // 1. Single attribute RHS
        var fdsSet = Core.MinimalCover.GetSingleAttributeRhsFds(parsedFds);
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
