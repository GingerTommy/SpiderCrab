namespace SpiderCrab.Agent
{
    using Serilog;
    using System.Web.Http;

    public class AgentController : ApiController
    {
        private readonly IScriptInvoker invoker;
        private readonly ILogger logger;

        public AgentController(IScriptInvoker invoker, ILogger logger)
        {
            this.invoker = invoker;
            this.logger = logger.ForContext<AgentController>();
        }

        [HttpPost]
        public IHttpActionResult Post(ExecuteScriptRequest request)
        {
            this.logger.Information(
                "{controller}.{method} invoked",
                nameof(AgentController),
                nameof(AgentController.Post));
            var result = this.invoker.Execute(request.Script);
            return this.Ok(result);
        }
    }
}