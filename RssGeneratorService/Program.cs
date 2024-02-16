using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RssGenerator.Services;

namespace RssGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                    configurationBuilder.AddJsonFile("appsettings.json", true);
                    configurationBuilder.AddJsonFile("appsettings.Development.json", true);
                    configurationBuilder.AddJsonFile("appsettings.Release.json", true);
                    configurationBuilder.AddEnvironmentVariables();
                    IConfigurationRoot configuration = configurationBuilder.Build();

                    // Register the configuration as a service
                    services.AddSingleton<IConfiguration>(configuration);

                    var connectionString = configuration.GetConnectionString("RssGenerator");
                    services.AddDbContext<RssGeneratorContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Singleton);

                    services.AddSingleton<IWebScrapperService, WebScrapperService>();

                    services.AddHttpClient();
                    services.AddLogging(c => c.AddConsole());

                    services.AddHostedService<Worker>();
                })
                .Build();

            host.Run();
        }
    }
}