using server.Utils;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Reflection;
using static server.Simulator.Logger;
using server.Simulator;
using Microsoft.EntityFrameworkCore;
using server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNet.Identity;

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
                    new JsonFormatter(),
                    "..\\logs\\debug-.json",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                .WriteTo.File(
                    new JsonFormatter(),
                    "..\\logs\\error-.json",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
                .WriteTo.File(
                    new JsonFormatter(),
                    "..\\logs\\fatal-.json",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                .WriteTo.File(
                    new JsonFormatter(),
                    "..\\logs\\info-.json",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Verbose)
                .WriteTo.File(
                    new JsonFormatter(),
                    "..\\logs\\verbose-.json",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                .WriteTo.File(
                    new JsonFormatter(),
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

            //_ = app.Services.GetRequiredService<Simulator.Simulator>().SimulateLoop();

            app.Run();
        }

        private static void InitilizeApp(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<Simulator.Simulator>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Simulator.Simulator>>();
                return new Simulator.Simulator(logger);
            });

            builder.Configuration.AddJsonFile("appsettings.json");

            builder.Services.AddDbContext<MallenomContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionUrl")));

            //builder.Services.AddScoped<DbUtils>();

            // https://learn.microsoft.com/ru-ru/aspnet/core/security/authorization/secure-data?view=aspnetcore-8.0
            builder.Services.AddIdentity<LoginModel, IdentityRole>(option =>
            {
                option.SignIn.RequireConfirmedAccount = true;
            })
                //.AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MallenomContext>();

            builder.Services.AddAuthorization(options =>
            {
                //options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    //.RequireAuthenticatedUser()
                    //.Build();
            });

            //private readonly SignInManager<LoginModel> _signInManager;
            //private readonly UserManager<LoginModel> _userManager;
            //builder.Services.AddScoped<SignInManager<LoginModel>>();
            //builder.Services.AddScoped<UserManager<LoginModel>>();
        }
    }
}
