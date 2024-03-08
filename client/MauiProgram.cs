using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using client.Pages;
using client.ViewModels;
using System.Reflection;
using client.Utils;

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


            var a = Assembly.GetExecutingAssembly();
            var appSettings = $"{a.GetName().Name}.Properties.appSettings.json";
            using var stream = a.GetManifestResourceStream(appSettings);

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();


            builder.Configuration.AddConfiguration(config);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<NetUtils>();

            builder.Services.AddTransient<ConnectionPage>();
            builder.Services.AddTransient<ConnectionPageViewModel>();

            builder.Services.AddTransient<AuthPage>();
            builder.Services.AddTransient<AuthPageViewModel>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();

            return builder.Build();
        }
    }
}
