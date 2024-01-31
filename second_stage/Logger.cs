using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog;

namespace second_stage
{
    public static class Logger
    {
        public static int day;
        public static int hour;
        public static ILogger logger;

        public static void CreateLogger()
        {
            var log = new LoggerConfiguration()
                .WriteTo.Console()

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

                // set default minimum level
                .MinimumLevel.Verbose()
                .CreateLogger();

            Log.Logger = log;
            logger = log;
        }
    }
}
