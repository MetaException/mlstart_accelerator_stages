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

                .WriteTo.File("debug.json",
                              restrictedToMinimumLevel: LogEventLevel.Debug,
                              rollingInterval: RollingInterval.Hour)

                .WriteTo.File("error.json",
                              restrictedToMinimumLevel: LogEventLevel.Error,
                              rollingInterval: RollingInterval.Hour)

                .WriteTo.File("fatal.json",
                              restrictedToMinimumLevel: LogEventLevel.Fatal,
                              rollingInterval: RollingInterval.Hour)

                .WriteTo.File("info.json",
                              restrictedToMinimumLevel: LogEventLevel.Information,
                              rollingInterval: RollingInterval.Hour)

                .WriteTo.File("verbose.json",
                              restrictedToMinimumLevel: LogEventLevel.Verbose,
                              rollingInterval: RollingInterval.Hour)

                .WriteTo.File("warning.json",
                              restrictedToMinimumLevel: LogEventLevel.Warning,
                              rollingInterval: RollingInterval.Hour)

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
