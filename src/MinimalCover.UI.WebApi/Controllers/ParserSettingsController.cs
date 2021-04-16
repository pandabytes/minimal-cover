using System;

using MinimalCover.Application.Parsers.Settings;
using MinimalCover.UI.WebApi.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MinimalCover.UI.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ParserSettingsController : ControllerBase
  {
    private readonly ParserSettings m_parserSettings;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="settings"></param>
    public ParserSettingsController(ParserSettings settings) => m_parserSettings = settings;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ParserSettingsDto))]
    public IActionResult GetParserSettings()
    {
      var settingsDto = new ParserSettingsDto { Settings = m_parserSettings };
      return Ok(settingsDto);
    }

  }
}
