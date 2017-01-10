namespace SpiderCrab.Agent
{
    using Microsoft.Owin.Hosting;
    using System;

    internal class ServiceController
    {
        private readonly string httpEndpoint;
        private IDisposable host;

        public ServiceController(IAppSettings settings)
        {
            this.httpEndpoint = settings.HttpListenerUri;
        }

        internal void Start()
        {
            this.host = WebApp.Start<Startup>(this.httpEndpoint);
        }

        internal void Stop()
        {
            this.host?.Dispose();
        }
    }
}