using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MinimalCover.Core;

namespace MinimalCover.Console
{
  public class Program
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
      var givenFds = new FunctionalDependency[5] {
        new FunctionalDependency("A", "D"),
        new FunctionalDependency("B,C", "A,D"),
        new FunctionalDependency("C", "B"),
        new FunctionalDependency("E", "A"),
        new FunctionalDependency("E", "D")
      };
      //var givenFds = new FunctionalDependency[3] {
      //      new FunctionalDependency("A", "B,C"),
      //      new FunctionalDependency("B", "C"),
      //      new FunctionalDependency("A,B", "D")
      //};
      //var givenFds = new FunctionalDependency[11] {
      //      new FunctionalDependency("A,B", "C"),
      //      new FunctionalDependency("C", "A"),
      //      new FunctionalDependency("B,C", "D"),
      //      new FunctionalDependency("A,C,D", "B,D"),
      //      new FunctionalDependency("D", "E"),
      //      new FunctionalDependency("D", "G"),
      //      new FunctionalDependency("B,E", "C"),
      //      new FunctionalDependency("C,G", "B"),
      //      new FunctionalDependency("C,G", "D"),
      //      new FunctionalDependency("C,E", "A"),
      //      new FunctionalDependency("C,E", "G")
      //    };

      // 1. Single attribute RHS
      var fdsSet = Core.MinimalCover.GetSingleAttributeRhsFds(givenFds);
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

    }
  }
}
