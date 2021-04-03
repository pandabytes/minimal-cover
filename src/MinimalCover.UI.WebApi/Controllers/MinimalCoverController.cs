using System;
using System.Linq;
using System.Collections.Generic;

using MinimalCover.Application.Parsers;
using MinimalCover.UI.WebApi.Services;
using MinimalCover.UI.WebApi.Models;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FunctionalDependencyDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestMessage))]
    public IActionResult FindMinimalCover(string format, [FromBody] string value)
    {
      var isValidFormat = Enum.TryParse(format, true, out ParseFormat parseFormat);
      if (!isValidFormat)
      {
        var badRequestMsg = new BadRequestMessage($"Format \"{format}\" is not one of the valid formats", new List<string>());
        return BadRequest(badRequestMsg);
      }

      try
      {
        var fds = m_mcService.FindMinimalCover(parseFormat, value);
        var fdsDto = fds.Select(fd => 
          new FunctionalDependencyDto { Left = fd.Left, Right = fd.Right });

        return Ok(fdsDto);
      }
      catch (Exception ex)
        when (ex is NotSupportedException || ex is ArgumentException || ex is ParserException)
      {
        var innerExMessage = ex.InnerException?.Message;
        var details = new List<string>();
        if (innerExMessage != null)
        {
          details.Add(innerExMessage);
        }
        var badRequestMsg = new BadRequestMessage(ex.Message, details);
        
        LogDebugException(ex);
        return BadRequest(badRequestMsg);
      }
    }

    /// <summary>
    /// Post request to find the minimal cover given <paramref name="functionalDependencies"/>
    /// </summary>
    /// <param name="functionalDependencies">Array of functional dependencies</param>
    /// <returns>Action result</returns>
    [HttpPost("json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FunctionalDependencyDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestMessage))]
    public IActionResult FindMinimalCover(FunctionalDependencyDto[] functionalDependencies)
    {
      try
      {
        var domainFds = functionalDependencies.Select(fd => fd.ToDomainFd()).ToHashSet();
        var fds = m_mcService.FindMinimalCover(domainFds);
        var fdsDto = fds.Select(fd => 
          new FunctionalDependencyDto { Left = fd.Left, Right = fd.Right });

        return Ok(fdsDto);
      }
      catch (Exception ex) 
        when (ex is ArgumentException || ex is ParserException)
      {
        var innerExMessage = ex.InnerException?.Message;
        var details = new List<string>();
        if (innerExMessage != null)
        {
          details.Add(innerExMessage);
        }
        var badRequestMsg = new BadRequestMessage(ex.Message, details);

        LogDebugException(ex);
        return BadRequest(badRequestMsg);
      }
    }

    /// <summary>
    /// Log exception stack trace in debug mode
    /// </summary>
    /// <param name="ex">Exception</param>
    private void LogDebugException(Exception ex)
    {
      var innerExMessage = ex.InnerException?.Message ?? "";
      m_logger.LogDebug($"Exception message      : {ex.Message}{Environment.NewLine}" +
                        $"Exception type         : {ex.GetType()}{Environment.NewLine}" +
                        $"Inner exception message: {innerExMessage}{Environment.NewLine}" +
                        $"Inner exception type   : {ex.InnerException?.GetType()}{Environment.NewLine}" +
                        ex.StackTrace);

    }
  }
}
