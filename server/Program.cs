using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Microsoft.EntityFrameworkCore;
using server.Models;
using Microsoft.AspNetCore.Identity;
using static cmd.Logger;
using cmd;
using Microsoft.Extensions.Options;

namespace server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Sink(new ListSink(logEntries))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                .WriteTo.File(
                    "..\\logs\\debug-.json",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                .WriteTo.File(
                    "..\\logs\\error-.json",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
                .WriteTo.File(
                    "..\\logs\\fatal-.json",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                .WriteTo.File(
                    "..\\logs\\info-.json",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Verbose)
                .WriteTo.File(
                    "..\\logs\\verbose-.json",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                .WriteTo.File(
                    "..\\logs\\warning-.json",
                    rollingInterval: RollingInterval.Hour))

                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();

            builder.Host.UseSerilog();

            InitilizeApp(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //_ = app.Services.GetRequiredService<Simulator>().SimulateLoop();

            app.Run();
        }

        private static void InitilizeApp(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<Simulator>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Simulator>>();
                return new Simulator(logger);
            });

            builder.Configuration.AddJsonFile("appsettings.json");

            builder.Services.AddDbContext<MallenomContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionUrl")));

            //builder.Services.AddScoped<DbUtils>();

            // https://learn.microsoft.com/ru-ru/aspnet/core/security/authorization/secure-data?view=aspnetcore-8.0
            builder.Services.AddDefaultIdentity<LoginModel>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<MallenomContext>();

            /*
            builder.Services.AddIdentity<LoginModel, IdentityRole>(option =>
            {
                option.SignIn.RequireConfirmedAccount = true;
            })
                //.AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MallenomContext>();
            */

            /*
            builder.Services.AddAuthorization(options =>
            {
                //options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    //.RequireAuthenticatedUser()
                    //.Build();
            });*/
        }
    }
}
