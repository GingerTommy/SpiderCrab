namespace SpiderCrab.Agent.Tests
{
    using System;
    using Xunit;

    public class ScriptInvokerTests
    {
        public class Execute
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData(" ")]
            public void ThrowsArgumentNullWhenNullScriptBlock(string scriptBlock)
            {
                // Arrange
                var sut = new ScriptInvoker();

                // Act, Assert
                Assert.Throws<ArgumentNullException>(
                    "scriptBlock",
                    () => sut.Execute(scriptBlock));
            }
        }
    }
}