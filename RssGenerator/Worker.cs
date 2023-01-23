namespace RssGenerator
{
    public class Worker : BackgroundService
    {
        private readonly IRssGeneratorService _rssGeneratorService;
        private readonly HttpClient _httpClient;
        private readonly Infrastructure.ILogger _logger;

        public Worker(IRssGeneratorService rssGeneratorService, Infrastructure.ILogger logger)
        {
            _rssGeneratorService = rssGeneratorService;
            _logger = logger;

            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    _logger.Info("Worker running at: {time}", DateTimeOffset.Now);

                    await _rssGeneratorService.UpdateRssFeedsAsync(cancellationToken);

                    await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);

                // Terminates this process and returns an exit code to the operating system.
                // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
                // performs one of two scenarios:
                // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
                // 2. When set to "StopHost": will cleanly stop the host, and log errors.
                //
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                Environment.Exit(1);
            }
        }

    }
}