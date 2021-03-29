using System;
using System.Linq;

using MinimalCover.Application.Parsers;

using MinimalCover.UI.WebApi.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MinimalCover.UI.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class MinimalCoverController : ControllerBase
  {
    private readonly ILogger<MinimalCoverController> m_logger;
    private readonly MinimalCoverService m_mcService;

    public MinimalCoverController(ILogger<MinimalCoverController> logger,
                                  MinimalCoverService mcService)
    {
      m_logger = logger;
      m_mcService = mcService;
    }

    [HttpGet("formats")]
    public IActionResult GetParseFormats()
    {
      var parseFormatNames = Enum.GetNames(typeof(ParseFormat));
      return Ok(parseFormatNames);
    }

    [HttpPost]
    public IActionResult FindMinimalCover(string format, [FromBody] string value)
    {
      var isValidFormat = Enum.TryParse(format, true, out ParseFormat parseFormat);
      if (!isValidFormat)
      {
        return BadRequest($"Format \"{format}\" is not one of the valid formats");
      }

      try
      {
        var fds = m_mcService.FindMinimalCover(parseFormat, value);
        return Ok(fds);
      }
      catch (Exception ex)
        when (ex is NotSupportedException || ex is ArgumentException || ex is ParserException)
      {
        var innerExMessage = ex.InnerException?.Message ?? "";
        var message = $"{ex.Message}. {innerExMessage}";
        return BadRequest(message);
      }
    }

    [HttpPost("json")]
    public IActionResult FindMinimalCover(Models.FunctionalDependency[] functionalDependencies)
    {
      try
      {
        var domainFds = functionalDependencies.Select(fd => fd.ToDomainFd()).ToHashSet();
        var fds = m_mcService.FindMinimalCover(domainFds);

        return Ok(fds);
      }
      catch (Exception ex) 
        when (ex is ArgumentException || ex is ParserException)
      {
        var innerExMessage = ex.InnerException?.Message ?? "";
        var message = $"{ex.Message}. {innerExMessage}";
        return BadRequest(message);
      }
    }



  }
}
