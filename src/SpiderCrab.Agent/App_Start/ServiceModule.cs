namespace SpiderCrab.Agent
{
    using Ninject.Modules;
    using Serilog;

    internal class ServiceModule : NinjectModule
    {
        private readonly IAppSettings settings;

        public ServiceModule(IAppSettings settings)
        {
            this.settings = settings;
        }

        public override void Load()
        {
            this.Bind<IAppSettings>()
                .ToConstant(this.settings);
            this.Bind<ILogger>()
                .ToConstant(Log.Logger);
            this.Bind<IScriptInvoker>()
                .To<ScriptInvoker>()
                .InSingletonScope();
        }
    }
}