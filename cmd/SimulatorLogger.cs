using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Collections.Concurrent;

namespace cmd
{
    public class SimulatorLogger
    {
        public class LogRecord
        {
            public string Message { get; set; }

            public LogRecord(string message)
            {
                Message = message;
            }
        }

        public static readonly ConcurrentQueue<LogRecord> logEntries = new ConcurrentQueue<LogRecord>();

        public static ILogger Logger;

        public static void CreateLogger()
        {
            var serilogLogger = new LoggerConfiguration()
                .WriteTo.Sink(new ListSink())

                .WriteTo.Logger(c =>
                    c.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                        .WriteTo.File("logs\\Simulator\\debug-.txt",
                              rollingInterval: RollingInterval.Hour))
                .WriteTo.Logger(c =>
                    c.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                        .WriteTo.File("logs\\Simulator\\error-.txt",
                              rollingInterval: RollingInterval.Hour))
                .WriteTo.Logger(c =>
                    c.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
                        .WriteTo.File("logs\\Simulator\\fatal-.txt",
                              rollingInterval: RollingInterval.Hour))
                .WriteTo.Logger(c =>
                    c.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                        .WriteTo.File("logs\\Simulator\\info-.txt",
                              rollingInterval: RollingInterval.Hour))
                .WriteTo.Logger(c =>
                    c.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Verbose)
                         .WriteTo.File("logs\\Simulator\\verbose-.txt",
                              rollingInterval: RollingInterval.Hour))
                .WriteTo.Logger(c =>
                    c.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                        .WriteTo.File("logs\\Simulator\\warning-.txt",
                              restrictedToMinimumLevel: LogEventLevel.Warning,
                              rollingInterval: RollingInterval.Hour))

                .MinimumLevel.Verbose()
                .CreateLogger();

            Logger = serilogLogger;
        }

        public class ListSink : ILogEventSink
        {

            public async void Emit(LogEvent logEvent)
            {
                // Преобразуем логов в строку и добавляем в список
                logEntries.Enqueue(new LogRecord(logEvent.Timestamp + " " + logEvent.RenderMessage()));
            }
        }
    }
}