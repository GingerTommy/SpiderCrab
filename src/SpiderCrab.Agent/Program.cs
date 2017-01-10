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

            HostFactory.Run(x =>
            {
                x.UseSerilog();
                x.UseNinject(new ServiceModule(Settings.Default));
                x.Service<ServiceController>(s =>
                {
                    s.ConstructUsingNinject();
                    s.WhenStarted(sc => sc.Start());
                    s.WhenStopped(sc => sc.Stop());
                });

                x.SetServiceName("SpiderCrab.Agent");
                x.SetDisplayName("SpiderCrab Agent");
                x.StartAutomaticallyDelayed();
            });
        }
    }
}