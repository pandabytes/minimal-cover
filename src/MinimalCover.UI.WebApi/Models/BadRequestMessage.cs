using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MinimalCover.UI.WebApi.Models
{
  /// <summary>
  /// Class containing message and details on why 
  /// the request is considered "bad"
  /// </summary>
  public class BadRequestMessage
  {
    [Required]
    public string Message { get; }

    [Required]
    public List<string> Details { get; }

    public BadRequestMessage(string message, List<string> details)
    {
      Message = message;
      Details = details;
    }

  }
}
