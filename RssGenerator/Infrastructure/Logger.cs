namespace RssGenerator.Infrastructure
{
    public class Logger : ILogger
    {
        private readonly NLog.ILogger _logger;
        private readonly ILogger<Worker> _consoleLogger;

        public Logger(NLog.ILogger logger, ILogger<Worker> consoleLogger)
        {
            this._logger = logger;
            _consoleLogger = consoleLogger;
        }

        public void Debug(string message, params object[] objects)
        {
            Console.WriteLine($"DEBUG: {message}: {objects}");
            _logger.Debug(message, objects);
            _consoleLogger.LogDebug(message, objects);
        }

        public void Error(string message, params object[] objects)
        {
            Console.WriteLine($"ERROR: {message}: {objects}");
            _logger.Error(message, objects);
            _consoleLogger.LogError(message, objects);
        }

        public void Exception(string requestName, Exception exception)
        {
            var message = $"Handled {requestName} threw an exception: {exception}";

            Console.WriteLine($"ERROR: {message}");
            _logger.Error(message);
            _consoleLogger.LogError(message);
        }

        public void Info(string message, params object[] objects)
        {
            Console.WriteLine($"INFO: {message}: {objects}");
            _logger.Info(message, objects);
            _consoleLogger.LogInformation(message, objects);
        }

        public void Warning(string message, params object[] objects)
        {
            Console.WriteLine($"WARNING: {message}: {objects}");
            _logger.Warn(message, objects);
            _consoleLogger.LogWarning(message, objects);
        }
    }
}