using System.Collections.Concurrent;
using DotNetEnv.Configuration;
using getting_service.Data;
using getting_service.DataBase.Context;
using getting_service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace getting_service;

internal static class Program
{
    public static ConcurrentQueue<string> MessageQueue { get; } = new();
    public static ConcurrentQueue<string> FilePathQueue { get; } = new();

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

            services.AddHostedService<TelegramService>();
            services.AddHostedService<SocatService>();
            services.AddHostedService<AutomaticQueryService>();
            services.AddHostedService<DatabaseService>();
        });

        builder.ConfigureLogging((context, loggingBuilder) =>
        {
            loggingBuilder.AddConsole();
        });

        await builder.RunConsoleAsync();
    }
}