namespace SpiderCrab.Agent
{
    using System.Collections.Generic;

    public class ExecuteScriptResponse
    {
        public string CorrelationId { get; set; }

        public IReadOnlyCollection<string> Results { get; set; }
    }
}