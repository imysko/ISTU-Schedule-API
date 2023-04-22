using getting_service.Controllers;
using getting_service.DataBase.Context;
using Microsoft.Extensions.Hosting;

namespace getting_service.Services;

public class DatabaseService : BackgroundService
{
    private readonly ScheduleDbController _dbController;
    
    public DatabaseService(ScheduleDbContext dbContext)
    {
        _dbController = new ScheduleDbController(dbContext);
    }

    private async Task UploadJsonToDatabase(string path)
    {
        await _dbController.LoadJson(path);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Starting database service");
        while (!stoppingToken.IsCancellationRequested)
        {
            while (Program.FilePathQueue.TryDequeue(out var path))
            {
                await UploadJsonToDatabase(path);
            }

            await Task.Delay(TimeSpan.FromMilliseconds(100), stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping database service");
        await base.StopAsync(cancellationToken);
    }
}