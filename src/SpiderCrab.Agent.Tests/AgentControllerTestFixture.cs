namespace SpiderCrab.Agent.Tests
{
    using Moq;
    using Serilog;
    using System;
    using System.Collections.Generic;

    public class AgentControllerTestFixture : IDisposable
    {
        public AgentControllerTestFixture()
        {
            this.ScriptInvoker = new Mock<IScriptInvoker>();
            this.ScriptInvoker.SetReturnsDefault<IReadOnlyCollection<string>>(
                new List<string>().AsReadOnly());
            this.Logger = new Mock<ILogger>();
            this.Logger.SetReturnsDefault(this.Logger.Object);
            this.AgentController = new AgentController(
                this.ScriptInvoker.Object, this.Logger.Object);
        }

        public AgentController AgentController { get; private set; }

        public Mock<ILogger> Logger { get; private set; }

        public Mock<IScriptInvoker> ScriptInvoker { get; private set; }

        public void Dispose()
        {
            this.AgentController?.Dispose();
        }
    }
}