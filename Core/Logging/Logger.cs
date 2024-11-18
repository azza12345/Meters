using Serilog;

namespace Core.Logging
{
    public class Logger : ILogger
    {
        private readonly LoggingDestination _destination;
        private readonly Serilog.Core.Logger _seqLogger;

        public Logger(LoggingDestination destination)
        {
            _destination = destination;

            
            if (_destination == LoggingDestination.Seq || _destination == LoggingDestination.Both)
            {
                _seqLogger = new LoggerConfiguration()
                    .WriteTo.Seq("http://localhost:5341") 
                    .CreateLogger();
            }
        }

        public void LogInfo(string message)
        {
            if (_destination == LoggingDestination.Console || _destination == LoggingDestination.Both)
            {
                Console.WriteLine($"INFO: {message}");
            }

            if (_destination == LoggingDestination.Seq || _destination == LoggingDestination.Both)
            {
                _seqLogger?.Information(message);
            }
        }

        public void LogError(string message)
        {
            if (_destination == LoggingDestination.Console || _destination == LoggingDestination.Both)
            {
                Console.WriteLine($"ERROR: {message}");
            }

            if (_destination == LoggingDestination.Seq || _destination == LoggingDestination.Both)
            {
                _seqLogger?.Error(message);
            }
        }
    }
}
