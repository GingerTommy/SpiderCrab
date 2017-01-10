namespace SpiderCrab.Agent
{
    using Properties;
    using Serilog;
    using Topshelf;
    using Topshelf.Ninject;

    internal static class Program
    {
        private static void Main()
        {
            Log.Logger = LogConfiguration.Default
                .CreateLogger();

            HostFactory.Run(host =>
            {
                host.UseSerilog();
                host.UseNinject(new ServiceModule(Settings.Default));
                host.Service<ServiceController>(service =>
                {
                    service.ConstructUsingNinject();
                    service.WhenStarted(sc => sc.Start());
                    service.WhenStopped(sc => sc.Stop());
                });

                host.SetServiceName("SpiderCrab.Agent");
                host.SetDisplayName("SpiderCrab Agent");
                host.RunAsLocalSystem();
                host.StartAutomaticallyDelayed();
            });
        }
    }
}