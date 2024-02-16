using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Collections.ObjectModel;

namespace second_stage
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

        public static readonly ObservableCollection<LogRecord> logEntries = new ObservableCollection<LogRecord>();

        public static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Sink(new ListSink(logEntries))

                .WriteTo.File(new JsonFormatter(),
                             "debug.json",
                              restrictedToMinimumLevel: LogEventLevel.Debug)

                .WriteTo.File(new JsonFormatter(),
                              "error.json",
                              restrictedToMinimumLevel: LogEventLevel.Error)

                .WriteTo.File(new JsonFormatter(),
                              "fatal.json",
                              restrictedToMinimumLevel: LogEventLevel.Fatal)

                .WriteTo.File(new JsonFormatter(),
                              "info.json",
                              restrictedToMinimumLevel: LogEventLevel.Information)

                .WriteTo.File(new JsonFormatter(),
                              "verbose.json",
                              restrictedToMinimumLevel: LogEventLevel.Verbose)

                .WriteTo.File(new JsonFormatter(),
                              "warning.json",
                              restrictedToMinimumLevel: LogEventLevel.Warning)

                .MinimumLevel.Verbose()
                .CreateLogger();
        }

        public class ListSink : ILogEventSink
        {
            private readonly ObservableCollection<LogRecord> _logList;

            public ListSink(ObservableCollection<LogRecord> logList)
            {
                _logList = logList ?? throw new ArgumentNullException(nameof(logList));
            }

            public void Emit(LogEvent logEvent)
            {
                // Преобразуем логов в строку и добавляем в список
                _logList.Add(new LogRecord(logEvent.RenderMessage()));
            }
        }

    }
}
