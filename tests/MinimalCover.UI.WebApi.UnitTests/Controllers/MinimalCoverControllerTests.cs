using System;
using System.Collections.Generic;
using Xunit;

using Moq;
using MinimalCover.Application;
using MinimalCover.Application.Parsers;
using MinimalCover.Application.Algorithms;
using MinimalCover.UI.WebApi.Controllers;
using MinimalCover.UI.WebApi.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MinimalCover.UI.WebApi.UnitTests.Controllers
{
  public class MinimalCoverControllerTests
  {
    public static readonly TheoryData<Exception> CaughtExceptionsForFindMcWithFormat =
      new()
      {
        new ArgumentException(),
        new NotSupportedException(),
        new ParserException("dummy_message")
      };

    public static readonly TheoryData<Exception> CaughtExceptionsForFindMc =
      new()
      {
        new ArgumentException(),
        new ParserException("dummy_message")
      };

    /// <summary>
    /// Object under test
    /// </summary>
    private readonly MinimalCoverController m_controller;

    // Mock objects
    private readonly Mock<ILogger<MinimalCoverController>> m_mockLogger;
    private readonly Mock<IMinimalCover> m_mockMinimalCover;
    private readonly GetParser m_mockGetParser;
    private readonly Mock<MinimalCoverApp> m_mockMinimalCoverApp;
    private readonly Mock<MinimalCoverService> m_mockMcService;

    public MinimalCoverControllerTests()
    {
      m_mockLogger = new();
      m_mockMinimalCover = new();
      m_mockGetParser = (parseFormat) => new Mock<IParser>().Object;
      m_mockMinimalCoverApp = new(m_mockMinimalCover.Object);
      m_mockMcService = new(m_mockMinimalCoverApp.Object, m_mockGetParser);
      m_controller = new MinimalCoverController(m_mockLogger.Object, m_mockMcService.Object);
    }

    [Fact]
    public void GetParseFormats_SimpleCall_ReturnsFormatArray()
    {
      // Arrange

      // Act
      var actionResult = m_controller.GetParseFormats();

      // Assert
      Assert.IsAssignableFrom<OkObjectResult>(actionResult);
      var objectResult = ((OkObjectResult)actionResult).Value;
      Assert.IsAssignableFrom<string[]>(objectResult);

      var actualFormatNames = (string[])objectResult;
      var expectedFormatNames = Enum.GetNames(typeof(ParseFormat));
      Assert.Equal(expectedFormatNames, actualFormatNames);
    }

    [Fact]
    public void FindMinimalCoverWithFormat_InvalidFormat_ReturnsBadRequest()
    {
      // Arrange

      // Act
      var actionResult = m_controller.FindMinimalCover("bad_format", "");

      // Assert
      Assert.IsAssignableFrom<BadRequestObjectResult>(actionResult);
      var objectResult = ((BadRequestObjectResult)actionResult).Value;
      Assert.IsAssignableFrom<string>(objectResult);
    }

    [Theory]
    [InlineData("Text")]
    [InlineData("text")]
    [InlineData("TEXT")]
    [InlineData("Json")]
    [InlineData("json")]
    [InlineData("JSON")]
    public void FindMinimalCoverWithFormat_ValidFormat_ReturnsOk(string format)
    {
      // Arrange - Mock method to return dummy object
      m_mockMinimalCoverApp
        .Setup(app => app.FindMinimalCover(It.IsAny<IParser>(), It.IsAny<string>()))
        .Returns(new HashSet<Domain.Models.FunctionalDependency>());

      // Act
      var actionResult = m_controller.FindMinimalCover(format, "dummy_value");

      // Assert
      Assert.IsAssignableFrom<OkObjectResult>(actionResult);
      var objectResult = ((OkObjectResult)actionResult).Value;
      Assert.IsAssignableFrom<ISet<Domain.Models.FunctionalDependency>>(objectResult);
    }

    [Theory]
    [MemberData(nameof(CaughtExceptionsForFindMcWithFormat))]
    public void FindMinimalCoverWithFormat_CaughtExceptionThrown_ReturnsBadRequest(Exception ex)
    {
      // Mock method to throw exception based on the given exception
      m_mockMinimalCoverApp
        .Setup(app => app.FindMinimalCover(It.IsAny<IParser>(), It.IsAny<string>()))
        .Throws(ex);

      // Act - use a valid text format
      var actionResult = m_controller.FindMinimalCover("text", "dummy_value");

      // Assert
      Assert.IsAssignableFrom<BadRequestObjectResult>(actionResult);
      var objectResult = ((BadRequestObjectResult)actionResult).Value;
      Assert.IsAssignableFrom<string>(objectResult);
    }

    [Fact]
    public void FindMinimalCover_ValidFuncDeps_ReturnsOk()
    {
      // Arrange - Mock method to return dummy object
      var fds = new Models.FunctionalDependency[] { 
        new Models.FunctionalDependency { Left = new HashSet<string>{ "a" }, Right = new HashSet<string>{ "b" } },
        new Models.FunctionalDependency { Left = new HashSet<string>{ "c" }, Right = new HashSet<string>{ "d", "e" } }
      };

      m_mockMinimalCoverApp
        .Setup(app => app.FindMinimalCover(It.IsAny<ISet<Domain.Models.FunctionalDependency>>()))
        .Returns(new HashSet<Domain.Models.FunctionalDependency>());

      // Act
      var actionResult = m_controller.FindMinimalCover(fds);

      // Assert
      Assert.IsAssignableFrom<OkObjectResult>(actionResult);
      var objectResult = ((OkObjectResult)actionResult).Value;
      Assert.IsAssignableFrom<ISet<Domain.Models.FunctionalDependency>>(objectResult);
    }

    [Theory]
    [MemberData(nameof(CaughtExceptionsForFindMc))]
    public void FindMinimalCover_CaughtExceptionThrown_ReturnsBadRequest(Exception ex)
    {
      // Arrange
      var fds = new Models.FunctionalDependency[] {
        new Models.FunctionalDependency { Left = new HashSet<string>{ "a" }, Right = new HashSet<string>{ "b" } },
        new Models.FunctionalDependency { Left = new HashSet<string>{ "c" }, Right = new HashSet<string>{ "d", "e" } }
      };

      // Mock method to throw exception based on the given exception
      m_mockMinimalCoverApp
        .Setup(app => app.FindMinimalCover(It.IsAny<ISet<Domain.Models.FunctionalDependency>>()))
        .Throws(ex);

      // Act
      var actionResult = m_controller.FindMinimalCover(fds);

      // Assert
      Assert.IsAssignableFrom<BadRequestObjectResult>(actionResult);
      var objectResult = ((BadRequestObjectResult)actionResult).Value;
      Assert.IsAssignableFrom<string>(objectResult);
    }

  }
}
