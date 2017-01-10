namespace SpiderCrab.Agent
{
    using Serilog;
    using System.Web.Http;

    public class AgentController : ApiController
    {
        private readonly ILogger logger;

        public AgentController(ILogger logger)
        {
            this.logger = logger.ForContext<AgentController>();
        }

        [HttpPost]
        public IHttpActionResult Post()
        {
            this.logger.Information(
                "{controller}.{method} invoked",
                nameof(AgentController),
                nameof(AgentController.Post));
            return this.Ok();
        }
    }
}