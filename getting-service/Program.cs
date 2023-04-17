using DotNetEnv.Configuration;
using getting_service.Data;
using getting_service.DataBase.Context;
using getting_service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace getting_service;

internal static class Program
{
    private const string AppSettingsFile = "appsettings.json";
    private const string TelegramEnvFile = "config/telegram.env";
    
    private const string TelegramSection = "TelegramConfig";
    
    private static async Task Main()
    {
        var builder = new HostBuilder();

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile(AppSettingsFile, false, true);
            config.AddDotNetEnv(TelegramEnvFile);
            config.AddEnvironmentVariables();
        });
        
        builder.ConfigureServices((hostingContext, services) =>
        {
            services.AddOptions<TelegramConfig>()
                .Bind(hostingContext.Configuration.GetSection(TelegramSection));

            services.Configure<TelegramConfig>(options 
                => hostingContext.Configuration.GetSection(TelegramSection).Bind(options));
            
            services.AddSingleton<IOptionsMonitor<TelegramConfig>, OptionsMonitor<TelegramConfig>>();

            services.AddDbContext<ScheduleDbContext>(options => 
                options.UseNpgsql(hostingContext.Configuration.GetConnectionString("ScheduleDB")));

            services.AddHostedService<GettingService>();
        });

        await builder.RunConsoleAsync();
    }
}