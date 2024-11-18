namespace Core.Logging
{
    public static class LoggerHelper
    {
        private static ILogger? _logger;

        
        public static void Initialize(ILogger logger)
        {
            _logger = logger;
        }


        public static void LogInfo(string message)
        {
            if (_logger == null)
                throw new InvalidOperationException("Logger not initialized. Call Initialize first.");

            _logger.LogInfo(message);
        }

        public static void LogError(Exception ex)
        {
            if (_logger == null)
                throw new InvalidOperationException("Logger not initialized. Call Initialize first.");

            _logger.LogError(ex.ToString());
        }
    }
}
