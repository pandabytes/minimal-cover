using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MinimalCover.UI.WebApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();

              //// Change the content path to where the assembly file is
              //var assemblyPath = Assembly.GetEntryAssembly()?.Location;
              //if (assemblyPath != null)
              //{
              //  var contentPath = assemblyPath.Substring(0, assemblyPath.LastIndexOf(@"\") + 1);
              //  webBuilder.UseContentRoot(contentPath);
              //}
            });
  }
}
