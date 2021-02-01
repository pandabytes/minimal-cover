
namespace MinimalCover.WebApp.Models
{
  /// <summary>
  /// Model representing functional dependencies. This
  /// is only used when transfer fd for request/response
  /// </summary>
  public class FunctionalDependencyString
  {
    public string InputFormat { get; set; }

    public string Value { get; set; }
  }
}
