using System;
using MinimalCover.Application.Parsers.Settings;
using System.ComponentModel.DataAnnotations;

namespace MinimalCover.UI.WebApi.Models
{
  public class ParserSettingsDto
  {
    [Required]
    public ParserSettings Settings { get; init; } = null!;
  }
}
