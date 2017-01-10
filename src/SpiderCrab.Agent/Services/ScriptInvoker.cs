namespace SpiderCrab.Agent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;

    public class ScriptInvoker : IScriptInvoker
    {
        public IReadOnlyCollection<string> Execute(string scriptBlock)
        {
            using (var shell = PowerShell.Create())
            {
                var output = shell.AddScript(scriptBlock).Invoke();
                if (shell.Streams.Error.Count > 0)
                {
                    var errors =
                        from error in shell.Streams.Error
                        select error.Exception;
                    throw new AggregateException(errors);
                }

                var results =
                    from result in output
                    select result.ToString();
                return results.ToList().AsReadOnly();
            }
        }
    }
}