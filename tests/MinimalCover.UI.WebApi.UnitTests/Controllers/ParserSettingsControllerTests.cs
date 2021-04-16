using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;

using MinimalCover.Application.Parsers.Settings;
using MinimalCover.UI.WebApi.Models;
using MinimalCover.UI.WebApi.Controllers;

namespace MinimalCover.UI.WebApi.UnitTests.Controllers
{
  public class ParserSettingsControllerTests
  {
    [Fact]
    public void GetParserSettings_SimpleCall_ReturnsParserSettingsDto()
    {
      // Arrange
      var mockParserSettings = new ParserSettings();
      var controller = new ParserSettingsController(mockParserSettings);

      // Act
      var actionResult = controller.GetParserSettings();

      // Assert
      Assert.IsAssignableFrom<OkObjectResult>(actionResult);
      var objectResult = ((OkObjectResult)actionResult).Value;
      Assert.IsAssignableFrom<ParserSettingsDto>(objectResult);
    }
  }
}
