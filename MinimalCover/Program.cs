using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MinimalCover
{
  public class Program
  {
    public static IEnumerable<FunctionalDependency> SingletonAttribute(FunctionalDependency fd)
    {
      var fds = new List<FunctionalDependency>();
      foreach (var attribute in fd.Right)
      {
        var newFd = new FunctionalDependency(fd.Left, attribute);
        fds.Add(newFd);
      }

      return fds;
    }

    public static void ComputeFdClosure(FunctionalDependency fd, IEnumerable<FunctionalDependency> allFds)
    {
      var otherFds = allFds.Where(f => f != fd);
      if (otherFds.Count() != allFds.Count() - 1)
      {
        throw new ArgumentException("Given fd must be in the collection allFds");
      }

      foreach (var attribute in fd.Left)
      {

      }
    }


    public static void Main(string[] args)
    {
      var allFds = new FunctionalDependency[6] {
        new FunctionalDependency("C", "T"),
        new FunctionalDependency("H,R", "C,D,A"),
        new FunctionalDependency("H,T", "R"),
        new FunctionalDependency("C,S", "G"),
        new FunctionalDependency("H,S", "R"),
        new FunctionalDependency("C,H", "R, X")
      };

      var r = new Relation(allFds);
      //Console.WriteLine(r.Attributes);
      //Console.WriteLine(allFds[0].Equals(new FunctionalDependency("C", "T")));

      var a = new AttributeSet("a,b,c");
      var b = new AttributeSet("a,b,c");
      var s = new HashSet<string>() { "a", "b", "c" };
      var c = new AttributeSet(s);

      Console.WriteLine(a == null);






      //var fds = SingletonAttribute(allFds[5]);
      //foreach (var x in fds)
      //{
      //  Console.WriteLine(x);
      //}

      //Console.WriteLine(allFds[0].IsOnLeft("C"));
      //Console.WriteLine(allFds[0].IsOnLeft("D"));
      //Console.WriteLine(allFds[1].IsOnLeft(new string[] { "H", "R" }));
      //Console.WriteLine(allFds[1].IsOnLeft(new string[] { "H", "R", "T" }));
      //Console.WriteLine(allFds[1].IsOnRight(new string[] { "C" }));

      //var newFds = SingletonAttribute<string>(fd2);
      //foreach (var fd in newFds)
      //{
      //  Console.WriteLine(fd);
      //}
    }
  }
}
