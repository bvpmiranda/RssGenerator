using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RssGeneratorApi.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RssGeneratorService
{
    public class Worker : BackgroundService
    {
        private readonly IWebScrapperService _webScrapperService;
        private readonly ILogger<Worker> _logger;

        public Worker(IWebScrapperService webScrapperService, ILogger<Worker> logger)
        {
            _webScrapperService = webScrapperService ?? throw new ArgumentNullException(nameof(webScrapperService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await _webScrapperService.RefreshArticlesAsync();

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}