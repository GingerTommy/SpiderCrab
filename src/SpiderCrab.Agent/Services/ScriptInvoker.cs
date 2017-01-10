namespace SpiderCrab.Agent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;

    public class ScriptInvoker : IScriptInvoker
    {
        public IReadOnlyCollection<string> Execute(string script)
        {
            using (var shell = PowerShell.Create())
            {
                shell.AddScript(script);
                var output = shell.Invoke();

                if (shell.Streams.Error.Count > 0)
                {
                    var errors = shell.Streams.Error
                        .Select(ex => ex.Exception);
                    throw new AggregateException(errors);
                }

                return output
                    .Select(o => o.ToString())
                    .ToList()
                    .AsReadOnly();
            }
        }
    }
}