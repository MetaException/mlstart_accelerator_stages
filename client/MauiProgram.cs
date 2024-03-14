using client.Pages;
using client.Utils;
using client.ViewModels;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            CreateLogger();

            builder.Services.AddSingleton<NetUtils>();

            builder.Services.AddTransient<ConnectionPage>();
            builder.Services.AddTransient<ConnectionPageViewModel>();

            builder.Services.AddTransient<AuthPage>();
            builder.Services.AddTransient<AuthPageViewModel>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();

            return builder.Build();
        }

        private static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                .WriteTo.File(
                    "logs\\debug-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                .WriteTo.File(
                    "logs\\error-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
                .WriteTo.File(
                    "logs\\fatal-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                .WriteTo.File(
                    "logs\\info-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Verbose)
                .WriteTo.File(
                    "logs\\verbose-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                .WriteTo.File(
                    "logs\\warning-.txt",
                    rollingInterval: RollingInterval.Hour))

                .MinimumLevel.Verbose()
                .CreateLogger();
        }
    }
}