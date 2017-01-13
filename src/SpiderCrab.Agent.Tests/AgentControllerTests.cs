namespace SpiderCrab.Agent.Tests
{
    using Moq;
    using System;
    using System.Web.Http.Results;
    using Xunit;

    public class AgentControllerTests : IClassFixture<AgentControllerTestFixture>
    {
        private readonly AgentControllerTestFixture fixture;

        public AgentControllerTests(AgentControllerTestFixture fixture)
        {
            this.fixture = fixture;
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
    }
}