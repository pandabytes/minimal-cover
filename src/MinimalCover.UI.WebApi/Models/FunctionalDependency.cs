using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MinimalCover.UI.WebApi.Models
{
  /// <summary>
  /// Model representing the functional dependency request/response in JSON
  /// </summary>
  /// <remarks>
  /// If <see cref="Domain.Models.FunctionalDependency"/> is updated, this class
  /// should be updated as well
  /// </remarks>
  public class FunctionalDependency
  {
    [Required]
    public ISet<string>? Left { get; set; }

    [Required]
    public ISet<string>? Right { get; set; }

    /// <summary>
    /// Convert this instance to the domain functional dependency object
    /// </summary>
    /// <remarks>
    /// If <see cref="Left"/> and/or <see cref="Right"/> is/are null then
    /// the returned object's Left and/or Right will be empty <see cref="HashSet{String}"/> 
    /// </remarks>
    /// <returns><see cref="Domain.Models.FunctionalDependency"/> object</returns>
    public Domain.Models.FunctionalDependency ToDomainFd()
    {
      var left = Left ?? new HashSet<string>();
      var right = Right ?? new HashSet<string>();
      return new Domain.Models.FunctionalDependency(left, right);
    }

  }

}
