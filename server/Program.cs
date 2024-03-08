using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Microsoft.EntityFrameworkCore;
using server.Models;
using Microsoft.AspNetCore.Identity;
using static cmd.Logger;
using cmd;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                .WriteTo.File(
                    "..\\logs\\AspNetCore\\debug-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                .WriteTo.File(
                    "..\\logs\\AspNetCore\\error-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
                .WriteTo.File(
                    "..\\logs\\AspNetCore\\fatal-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                .WriteTo.File(
                    "..\\logs\\AspNetCore\\info-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Verbose)
                .WriteTo.File(
                    "..\\logs\\AspNetCore\\verbose-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                .WriteTo.File(
                    "..\\logs\\AspNetCore\\warning-.txt",
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

            app.UseRouting();
            app.UseSerilogRequestLogging();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            _ = app.Services.GetRequiredService<Simulator>().SimulateLoop();

            var configSection = builder.Configuration.GetSection("ConnectionConfiguration");

            app.Run($"https://{configSection["ServerIp"]}:{configSection["ServerPort"]}");
        }

        private static void InitilizeApp(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.AddSingleton<Simulator>();

            builder.Configuration.AddJsonFile("appsettings.json");

            builder.Services.AddDbContext<MallenomContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionUrl")));

            // https://learn.microsoft.com/ru-ru/aspnet/core/security/authorization/secure-data?view=aspnetcore-8.0
            builder.Services.AddIdentity<LoginModel, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<MallenomContext>();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("CCEA5D06F64497D9CCB548B70B024ASGESGHES5H50")),
                    ValidateIssuer = false, 
                    ValidateAudience = false, 
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
