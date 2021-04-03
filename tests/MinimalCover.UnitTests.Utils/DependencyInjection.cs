using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using MinimalCover.Infrastructure;

namespace MinimalCover.UnitTests.Utils
{
  /// <summary>
  /// This class that stores all dependencies that is 
  /// required for testing
  /// </summary>
  public class DependencyInjection
  {
    /// <summary>
    /// Ther serivce provider
    /// </summary>
    public ServiceProvider Provider { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="configuration">Configuration to configure the parsers</param>
    public DependencyInjection(IConfiguration configuration)
    {
      var services = new ServiceCollection();
      services.AddParsers(configuration)
              .AddMinimalCover();
      Provider = services.BuildServiceProvider();  
    }

  }
}
