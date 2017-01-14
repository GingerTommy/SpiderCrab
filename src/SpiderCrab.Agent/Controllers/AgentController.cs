namespace SpiderCrab.Agent
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Serilog;

    public class AgentController : ApiController
    {
        private readonly IScriptInvoker invoker;
        private readonly ILogger logger;

        public AgentController(IScriptInvoker scriptInvoker, ILogger logger)
        {
            if (scriptInvoker == null)
            {
                throw new ArgumentNullException(nameof(scriptInvoker));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.invoker = scriptInvoker;
            this.logger = logger.ForContext<AgentController>();
        }

        [HttpPost]
        public IHttpActionResult Post(ExecuteScriptRequest request)
        {
            if (request == null)
            {
                return this.BadRequest();
            }

            if (string.IsNullOrWhiteSpace(request.CorrelationId))
            {
                request.CorrelationId = Guid.NewGuid().ToString();
            }

            var logContext = this.logger.ForContext("CorrelationId", request.CorrelationId);
            logContext.Information("{request} received", nameof(ExecuteScriptRequest));
            if (string.IsNullOrWhiteSpace(request.ScriptBlock))
            {
                var message = string.Format("{0} cannot be empty", nameof(ExecuteScriptRequest.ScriptBlock));
                return this.BadRequest(message);
            }

            try
            {
                var results = this.invoker.Execute(request.ScriptBlock);
                var response = new ExecuteScriptResponse
                {
                    CorrelationId = request.CorrelationId,
                    Results = results.ToArray()
                };
                return this.Ok(response);
            }
            catch (AggregateException ex)
            {
                logContext.Error(ex, "{message}", ex.Message);
                var errors =
                    from error in ex.InnerExceptions
                    select error.Message;
                var message = string.Join(Environment.NewLine, errors);
                return this.BadRequest(message);
            }
            catch (Exception ex)
            {
                logContext.Error(ex, "{message}", ex.Message);
                return this.InternalServerError(ex);
            }
            finally
            {
                logContext.Information("Script execution complete");
            }
        }
    }
}