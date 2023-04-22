using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace getting_service.Services;

public class SocatService : BackgroundService
{
    private readonly Socket _listener;
    
    public SocatService()
    {
        _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _listener.Bind(new IPEndPoint(IPAddress.Any, 5723));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Starting socat service");
        _listener.Listen(10);

        while (!stoppingToken.IsCancellationRequested)
        {
            var client = await _listener.AcceptAsync(stoppingToken);
            Console.WriteLine($"Client connected: {client.RemoteEndPoint}");

            _ = Task.Run(async () =>
            {
                try
                {
                    var buffer = new byte[1024];
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var bytesRead = await client.ReceiveAsync(buffer, SocketFlags.None, stoppingToken);
                        if (bytesRead == 0) break;
                        var message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        Console.Write($"Received message from {client.RemoteEndPoint}: {message}");
                        Program.MessageQueue.Enqueue(message.Trim());
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Socket error: {ex.SocketErrorCode}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                }

                Console.WriteLine($"Client disconnected: {client.RemoteEndPoint}");
            }, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping socat service");
        _listener.Dispose();
        await base.StopAsync(cancellationToken);
    }
}