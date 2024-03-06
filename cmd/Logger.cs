using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace cmd
{
    public class Logger
    {
        public class LogRecord
        {
            public string Message { get; set; }

            public LogRecord(string message)
            {
                Message = message;
            }
        }

        public static readonly ConcurrentBag<LogRecord> logEntries = new ConcurrentBag<LogRecord>();

        public static ILogger<Simulator> logger;

        public static void CreateLogger()
        {
            var serilogLogger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Sink(new ListSink(logEntries))

                .WriteTo.File("..\\logs\\Simulator\\debug-.txt",
                              restrictedToMinimumLevel: LogEventLevel.Debug,
                              rollingInterval: RollingInterval.Hour)

                .WriteTo.File("..\\logs\\Simulator\\error-.txt",
                              restrictedToMinimumLevel: LogEventLevel.Error,
                              rollingInterval: RollingInterval.Hour)

                .WriteTo.File("..\\logs\\Simulator\\fatal-.txt",
                              restrictedToMinimumLevel: LogEventLevel.Fatal,
                              rollingInterval: RollingInterval.Hour)

                .WriteTo.File("..\\logs\\Simulator\\info-.txt",
                              restrictedToMinimumLevel: LogEventLevel.Information,
                              rollingInterval: RollingInterval.Hour)

                .WriteTo.File("..\\logs\\Simulator\\verbose-.txt",
                              restrictedToMinimumLevel: LogEventLevel.Verbose,
                              rollingInterval: RollingInterval.Hour)

                .WriteTo.File("..\\logs\\Simulator\\warning-.txt",
                              restrictedToMinimumLevel: LogEventLevel.Warning,
                              rollingInterval: RollingInterval.Hour)

                .MinimumLevel.Verbose()
                .CreateLogger();

            logger = new LoggerFactory().AddSerilog(serilogLogger).CreateLogger<Simulator>();
        }

        public class ListSink : ILogEventSink
        {
            private readonly ConcurrentBag<LogRecord> _logList;

            public ListSink(ConcurrentBag<LogRecord> logList)
            {
                _logList = logList ?? throw new ArgumentNullException(nameof(logList));
            }

            public void Emit(LogEvent logEvent)
            {
                // Преобразуем логов в строку и добавляем в список
                _logList.Add(new LogRecord(logEvent.Timestamp + " " + logEvent.RenderMessage()));
            }
        }

    }
}
