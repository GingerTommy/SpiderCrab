﻿namespace SpiderCrab.Agent
{
    public class ExecuteScriptRequest
    {
        public string CorrelationId { get; set; }

        public string ScriptBlock { get; set; }
    }
}