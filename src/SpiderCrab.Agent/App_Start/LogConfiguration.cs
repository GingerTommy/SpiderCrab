namespace SpiderCrab.Agent
{
    using Serilog;

    internal class LogConfiguration
    {
        public static LoggerConfiguration Default
        {
            get
            {
                return new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.ColoredConsole()
                    .WriteTo.RollingFile(@"C:\Logs\SpiderCrab-{Date}.log");
            }
        }
    }
}