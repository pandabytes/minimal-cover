using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalCover.Domain.Models;

namespace MinimalCover.UI.WebApi.Models
{
  public class FunctionalDependency
  {
    public ISet<string>? Left { get; set; }

    public ISet<string>? Right { get; set; }

    public Domain.Models.FunctionalDependency ToDomainFd()
    {
      var left = Left ?? new HashSet<string>();
      var right = Right ?? new HashSet<string>();
      return new Domain.Models.FunctionalDependency(left, right);
    }

  }

}
