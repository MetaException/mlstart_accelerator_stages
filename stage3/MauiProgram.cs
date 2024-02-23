using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using stage3.Pages;
using stage3.Utils;
using stage3.ViewModels;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace stage3
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

            builder.Services.AddSingleton<MallenomContext>(new MallenomContext(config));
            builder.Services.AddSingleton<DbUtils>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();

            builder.Services.AddTransient<AuthPage>();
            builder.Services.AddTransient<AuthPageViewModel>();

            return builder.Build();
        }
    }
}
