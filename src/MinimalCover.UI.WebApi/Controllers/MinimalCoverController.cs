using System;
using System.Linq;
using System.Collections.Generic;

using MinimalCover.Domain.Models;
using MinimalCover.Application.Parsers;
using MinimalCover.UI.WebApi.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace MinimalCover.UI.WebApi.Controllers
{
  /// <summary>
  /// Minimal Cover Controller
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class MinimalCoverController : ControllerBase
  {
    private readonly ILogger<MinimalCoverController> m_logger;
    private readonly MinimalCoverService m_mcService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="mcService">The minimal cover service</param>
    public MinimalCoverController(ILogger<MinimalCoverController> logger,
                                  MinimalCoverService mcService)
    {
      m_logger = logger;
      m_mcService = mcService;
    }

    /// <summary>
    /// Get all the available formats that can be parsed
    /// </summary>
    /// <returns></returns>
    [HttpGet("formats")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string[]))]
    public IActionResult GetParseFormats()
    {
      var parseFormatNames = Enum.GetNames(typeof(ParseFormat));
      return Ok(parseFormatNames);
    }

    /// <summary>
    /// Post request to find the minimal cover by specifying <paramref name="format"/>
    /// and <paramref name="value"/>
    /// </summary>
    /// <param name="format">Format the <paramref name="value"/> is in</param>
    /// <param name="value">String value to be parsed</param>
    /// <returns>Action result</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ISet<FunctionalDependency>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
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
        m_logger.LogDebug($"Exception type: {ex.GetType()}{Environment.NewLine}" + 
                          $"{message}{Environment.NewLine}{ex.StackTrace}");
        return BadRequest(message);
      }
    }

    /// <summary>
    /// Post request to find the minimal cover given <paramref name="functionalDependencies"/>
    /// </summary>
    /// <param name="functionalDependencies">Array of functional dependencies</param>
    /// <returns>Action result</returns>
    [HttpPost("json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ISet<FunctionalDependency>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
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
        m_logger.LogDebug($"Exception type: {ex.GetType()}{Environment.NewLine}" +
                          $"{message}{Environment.NewLine}{ex.StackTrace}");
        return BadRequest(message);
      }
    }

  }
}
