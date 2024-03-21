using cmd;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;

namespace server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            CreateLogger();

            ConfigureBuilder(builder);
            ConfigurateWithAppSettings(builder);

            var app = builder.Build();

            ConfigureApp(app);

            _ = Simulator.SimulateLoop();

            _ = CheckDatabaseConnection(app); //Вызовет краш когда установленный порт занят

            app.Run();
        }

        private static void ConfigureBuilder(WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog();

            builder.Services.AddControllers();

            builder.Configuration.AddJsonFile("appsettings.json");

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddDbContext<MallenomContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionUrl")));

            //builder.Services.AddHealthChecks()
            //    .AddDbContextCheck<MallenomContext>();

            // https://learn.microsoft.com/ru-ru/aspnet/core/security/authorization/secure-data?view=aspnetcore-8.0
            builder.Services.AddIdentity<IdentityUser<int>, IdentityRole<int>>(options =>
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

        private static void ConfigureApp(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                //app.UseExceptionHandler("/Home/Error"); у нас нет views
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

            //app.MapHealthChecks("/healtha");
        }

        private static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                .WriteTo.File(
                    "logs\\AspNetCore\\debug-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                .WriteTo.File(
                    "logs\\AspNetCore\\error-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
                .WriteTo.File(
                    "logs\\AspNetCore\\fatal-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                .WriteTo.File(
                    "logs\\AspNetCore\\info-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Verbose)
                .WriteTo.File(
                    "logs\\AspNetCore\\verbose-.txt",
                    rollingInterval: RollingInterval.Hour))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                .WriteTo.File(
                    "logs\\AspNetCore\\warning-.txt",
                    rollingInterval: RollingInterval.Hour))

                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static void ConfigurateWithAppSettings(WebApplicationBuilder builder)
        {
            Simulator.tasksDelay = Convert.ToDouble(builder.Configuration.GetRequiredSection("SimulatorConfiguration")["TasksDelayInSeconds"]);
            SimulatorLogger.MaxLogsCount = Convert.ToInt32(builder.Configuration.GetRequiredSection("SimulatorConfiguration")["MaxLogsCount"]);

            var configSection = builder.Configuration.GetSection("ConnectionConfiguration");

            builder.WebHost.UseUrls($"https://{configSection["ServerIp"]}:{configSection["ServerPort"]}");
        }

        private static async Task CheckDatabaseConnection(WebApplication app)
        {
            // Получаем доступ к контейнеру DI через IServiceProvider
            var serviceProvider = app.Services.GetRequiredService<IServiceProvider>();

            // Извлекаем MallenomContext из контейнера DI
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MallenomContext>();

                bool isConnected = await context.Database.CanConnectAsync();

                if (isConnected)
                {
                    Log.Information("Successfully connect to database server");
                }
                else
                {
                    Log.Error("Can't connected to the database server");
                }
            }
        }
    }
}