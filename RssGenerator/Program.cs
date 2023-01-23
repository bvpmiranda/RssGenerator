using NLog;

namespace RssGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .UseWindowsService(options =>
                {
                    options.ServiceName = "RSS Generator";
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<Infrastructure.ILogger, Infrastructure.Logger>();
                    services.AddSingleton<NLog.ILogger>(x => LogManager.GetCurrentClassLogger());
                    services.AddSingleton<IRssGeneratorService, RssGeneratorService>();

                    services.AddHostedService<Worker>();
                })
                .Build();

            host.Run();
        }
    }
}