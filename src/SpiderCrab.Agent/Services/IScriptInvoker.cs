namespace SpiderCrab.Agent
{
    using System.Collections.Generic;

    public interface IScriptInvoker
    {
        IReadOnlyCollection<string> Execute(string script);
    }
}