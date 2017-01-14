namespace SpiderCrab.Agent.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Results;
    using Moq;
    using Serilog;
    using Xunit;

    public class AgentControllerTests : IClassFixture<AgentControllerTestFixture>
    {
        private readonly AgentControllerTestFixture fixture;

        public AgentControllerTests(AgentControllerTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void ThrowsArgumentNullOnNullScriptInvoker()
        {
            // Arrange
            var logger = new Mock<ILogger>().Object;

            // Act, Assert
            var ex = Assert.Throws<ArgumentNullException>(
                "scriptInvoker",
                () => new AgentController(null, logger));
        }

        [Fact]
        public void ThrowsArgumentNullOnNullLogger()
        {
            // Arrange
            var scriptInvoker = new Mock<IScriptInvoker>().Object;

            // Act, Assert
            var ex = Assert.Throws<ArgumentNullException>(
                "logger",
                () => new AgentController(scriptInvoker, null));
        }

        [Fact]
        private void ReturnsBadRequestOnNullRequest()
        {
            // Act
            var result = this.fixture.AgentController.Post(null);

            // Assert
            var response = Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ReturnsBadRequestOnEmptyScript(string scriptBlock)
        {
            // Arrange
            var request = new ExecuteScriptRequest
            {
                CorrelationId = Guid.NewGuid().ToString(),
                ScriptBlock = scriptBlock
            };

            // Act
            var result = this.fixture.AgentController.Post(request);

            // Assert
            var response = Assert.IsType<BadRequestErrorMessageResult>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CreatesCorrelationIdWhenNull(string correlationId)
        {
            // Arrange
            var request = new ExecuteScriptRequest
            {
                CorrelationId = correlationId,
                ScriptBlock = "Get-Process"
            };

            // Act
            var result = this.fixture.AgentController.Post(request);

            // Assert
            var response = Assert.IsType<OkNegotiatedContentResult<ExecuteScriptResponse>>(result);
            Assert.NotNull(response.Content.CorrelationId);
        }

        [Fact]
        public void ReturnsValidCorrelationId()
        {
            // Arrange
            var request = new ExecuteScriptRequest
            {
                CorrelationId = Guid.NewGuid().ToString(),
                ScriptBlock = "Get-Process"
            };

            // Act
            var result = this.fixture.AgentController.Post(request);

            // Assert
            var response = Assert.IsType<OkNegotiatedContentResult<ExecuteScriptResponse>>(result);
            Assert.Equal(request.CorrelationId, response.Content.CorrelationId);
        }

        [Fact]
        public void EnrichesLogsWithCorrelationId()
        {
            // Arrange
            var request = new ExecuteScriptRequest
            {
                CorrelationId = Guid.NewGuid().ToString(),
                ScriptBlock = "Get-Process"
            };

            // Act
            var result = this.fixture.AgentController.Post(request);

            // Assert
            this.fixture.Logger.Verify(
                l => l.ForContext("CorrelationId", request.CorrelationId, false),
                Times.Once);
        }

        [Fact]
        public void ReturnsInvocationErrorsAsBadRequest()
        {
            // Arrange
            var scriptInvoker = new Mock<IScriptInvoker>();
            scriptInvoker.SetReturnsDefault<IReadOnlyCollection<string>>(
                new List<string>().AsReadOnly());
            scriptInvoker
                .Setup(si => si.Execute(It.IsAny<string>()))
                .Throws(new AggregateException(new NullReferenceException("ABC")));
            var logger = new Mock<ILogger>();
            logger.SetReturnsDefault(logger.Object);
            var sut = new AgentController(
                scriptInvoker.Object, logger.Object);
            var request = new ExecuteScriptRequest
            {
                CorrelationId = Guid.NewGuid().ToString(),
                ScriptBlock = "Get-Process"
            };

            // Act
            var result = sut.Post(request);

            // Assert
            var response = Assert.IsType<BadRequestErrorMessageResult>(result);
            Assert.Contains("ABC", response.Message);
        }

        [Fact]
        public void ReturnsUnexpectedErrorsAsException()
        {
            // Arrange
            var scriptInvoker = new Mock<IScriptInvoker>();
            scriptInvoker.SetReturnsDefault<IReadOnlyCollection<string>>(
                new List<string>().AsReadOnly());
            scriptInvoker
                .Setup(si => si.Execute(It.IsAny<string>()))
                .Throws(new NullReferenceException("ABC"));
            var logger = new Mock<ILogger>();
            logger.SetReturnsDefault(logger.Object);
            var sut = new AgentController(
                scriptInvoker.Object, logger.Object);
            var request = new ExecuteScriptRequest
            {
                CorrelationId = Guid.NewGuid().ToString(),
                ScriptBlock = "Get-Process"
            };

            // Act
            var result = sut.Post(request);

            // Assert
            var response = Assert.IsType<ExceptionResult>(result);
            Assert.IsType<NullReferenceException>(response.Exception);
        }
    }
}