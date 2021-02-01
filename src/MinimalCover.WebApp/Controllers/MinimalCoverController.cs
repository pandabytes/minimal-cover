using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MinimalCover.Core.Parsers;

namespace MinimalCover.WebApp.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class MinimalCoverController : ControllerBase
  {
    private readonly ILogger m_logger;

    public MinimalCoverController(ILogger<MinimalCoverController> logger)
    {
      m_logger = logger;
    }

    [HttpPost]
    public IActionResult GetMinimalCover(Models.FunctionalDependencyString fd)
    {
      object inputFormat;
      var validFormat = Enum.TryParse(typeof(InputFormat), fd.InputFormat, true, out inputFormat);

      if (!validFormat)
      {
        return StatusCode(422, $"Bad input format \"{fd.InputFormat}\"");
      }

      try
      {
        var fds = Parser.Parse((InputFormat)inputFormat, fd.Value);
        var minimalCover = Core.MinimalCover.FindMinimalCover(fds);
        var minimalCoverStr = string.Join(',', minimalCover);
        m_logger.LogInformation($"[POST]: {minimalCoverStr}");
        return Ok(minimalCover);
      }
      catch (ArgumentException ex)
      {
        return StatusCode(422, ex.Message);
      }
    }
  }
}
