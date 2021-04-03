using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using MinimalCover.Domain.Models;

namespace MinimalCover.UI.WebApi.Models
{
  /// <summary>
  /// DTO object representing the functional dependency request/response in JSON
  /// </summary>
  /// <remarks>
  /// If <see cref="FunctionalDependency"/> is updated, this class
  /// should be updated as well.
  /// <see cref="Left"/> and <see cref="Right"/> are nonnullable and since
  /// they are marked with <see cref="RequiredAttribute"/>, the MVC validation
  /// framework will ensure that these properties will not be null. Hence it's
  /// not recommended to programmatically construct this class
  /// </remarks>
  public class FunctionalDependencyDto
  {
    [Required]
    public ISet<string> Left { get; set; } = null!;

    [Required]
    public ISet<string> Right { get; set; } = null!;

    /// <summary>
    /// Convert this instance to the domain functional dependency object
    /// </summary>
    /// <returns><see cref="FunctionalDependency"/> object</returns>
    public FunctionalDependency ToDomainFd()
      => new FunctionalDependency(Left, Right);
  }

}
