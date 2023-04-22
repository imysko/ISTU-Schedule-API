using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;

namespace getting_service.Services;

[SuppressMessage("ReSharper", "AsyncVoidLambda")]
public class AutomaticQueryService : BackgroundService
{
    private readonly Timer _dailyTimer;
    private readonly Timer _monthlyTimer;

    public AutomaticQueryService()
    {
        _dailyTimer = CreateDailyTimer();
        _monthlyTimer = CreateMonthlyTimer();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Starting automatic query service");
        
        await Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping automatic query service");

        await _dailyTimer.DisposeAsync();
        await _monthlyTimer.DisposeAsync();
        
        await base.StopAsync(cancellationToken);
    }

    private Timer CreateDailyTimer()
    {
        var startTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 2, 0, 0, DateTimeKind.Utc);
        if (startTime < DateTime.UtcNow)
        {
            startTime = startTime.AddDays(1);
        }

        var interval = TimeSpan.FromDays(1);
        
        return new Timer(async _ =>
        {
            await RunDailyTask();
        }, null, startTime - DateTime.UtcNow, interval);
    }

    private Timer CreateMonthlyTimer()
    {
        var startTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 3, 0, 0, DateTimeKind.Utc);
        if (startTime < DateTime.UtcNow)
        {
            startTime = startTime.AddDays(30);
        }

        var interval = TimeSpan.FromDays(30);
        
        return new Timer(async _ =>
        {
            await RunMonthlyTask();
        }, null, startTime - DateTime.UtcNow, interval);
    }

    private Task RunDailyTask()
    {
        Program.MessageQueue.Enqueue("schedule two weeks");
        return Task.CompletedTask;
    }

    private Task RunMonthlyTask()
    {
        Program.MessageQueue.Enqueue("institutes");
        Program.MessageQueue.Enqueue("groups");
        Program.MessageQueue.Enqueue("classrooms");
        Program.MessageQueue.Enqueue("lessons time");
        Program.MessageQueue.Enqueue("teachers");
        Program.MessageQueue.Enqueue("disciplines");
        Program.MessageQueue.Enqueue("other disciplines");
        Program.MessageQueue.Enqueue("schedule three months");
        return Task.CompletedTask;
    }
}