using getting_service.Controllers;
using getting_service.Data;
using getting_service.Data.Models;
using getting_service.DataBase.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using TdLib;
using TdLib.Bindings;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace getting_service.Services;

public class GettingService : BackgroundService
{
    private readonly IConfigurationRoot _config;
    private readonly IOptionsMonitor<TelegramConfig> _optionsMonitor;

    private readonly ScheduleDbContext _context;
    private readonly ScheduleDbController _controller;
    
    private const string ApplicationVersion = "1.0.0";

    private static TdClient Client;
    private static readonly ManualResetEventSlim ReadyToAuthenticate = new();

    private static bool _authNeeded;
    private bool _isEnteredCode = false;
    private static bool _passwordNeeded;

    private static UpdateDate _updateDate = new();

    private static readonly string RootPath = 
        Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName 
        ?? string.Empty;

    public GettingService(IConfiguration config, ScheduleDbContext context, 
        IOptionsMonitor<TelegramConfig> optionsMonitor)
    {
        _config = (IConfigurationRoot)config;
        _optionsMonitor = optionsMonitor;

        // _context = context;
        _context = new ScheduleDbContext(_config.GetConnectionString("ScheduleDB")!);
        _controller = new ScheduleDbController(_context);
        
        Console.WriteLine("Start");
        Console.WriteLine($"Api Id: {_optionsMonitor.CurrentValue.ApiId}");
        Console.WriteLine($"Api hash: {_optionsMonitor.CurrentValue.ApiHash}");
        Console.WriteLine($"Phone number: {_optionsMonitor.CurrentValue.PhoneNumber}");
        Console.WriteLine($"ChatBot Id: {_optionsMonitor.CurrentValue.ChatBotId}");
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Client = new TdClient();

        Client.Bindings.SetLogVerbosityLevel(TdLogLevel.Fatal);
        
        Client.UpdateReceived += async (_, update) => { await ProcessUpdates(update); };

        ReadyToAuthenticate.Wait();

        if (_authNeeded)
        {
            await HandleAuthentication();
        }

        var currentUser = await GetCurrentUser();

        var fullUserName = $"{currentUser.FirstName} {currentUser.LastName}".Trim();
        Console.WriteLine($"Successfully logged in as [{currentUser.Id}] / [@{currentUser.Username}] / [{fullUserName}]");

        const int channelLimit = 5;
        var channels = GetChannels(channelLimit);

        Console.WriteLine($"Top {channelLimit} channels:");

        await foreach (var channel in channels)
        {
            Console.WriteLine($"[{channel.Id}] -> [{channel.Title}] ({channel.UnreadCount} messages unread)");
        }

        await GettingData();
    }
    
    private async Task HandleAuthentication()
    {
        Console.WriteLine("Await code");
        await Client.ExecuteAsync(new TdApi.SetAuthenticationPhoneNumber
        {
            PhoneNumber = _optionsMonitor.CurrentValue.PhoneNumber
        });

        var envFileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
        ChangeToken.OnChange(() => envFileProvider.Watch("config/telegram.env"), async () =>
        {
            _config.Reload();
            _isEnteredCode = true;
        });
        
        while (true)
        {
            if (_isEnteredCode)
            {
                var code = _optionsMonitor.CurrentValue.LoginCode;
                Console.WriteLine($"Secret code: {code}");
            
                await Client.ExecuteAsync(new TdApi.CheckAuthenticationCode
                {
                    Code = code
                });
                break;
            }
        }
    }

    private async Task ProcessUpdates(TdApi.Update update)
    {
        switch (update)
        {
            case TdApi.Update.UpdateAuthorizationState { AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters }:
                var filesLocation = Path.Combine(AppContext.BaseDirectory, "db");
                await Client.ExecuteAsync(new TdApi.SetTdlibParameters
                {
                    Parameters = new TdApi.TdlibParameters
                    {
                        ApiId = int.Parse(_optionsMonitor.CurrentValue.ApiId),
                        ApiHash = _optionsMonitor.CurrentValue.ApiHash,
                        DeviceModel = "PC",
                        SystemLanguageCode = "en",
                        ApplicationVersion = ApplicationVersion,
                        DatabaseDirectory = filesLocation,
                        FilesDirectory = filesLocation,
                    }
                });
                break;

            case TdApi.Update.UpdateAuthorizationState { AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitEncryptionKey }:
                await Client.ExecuteAsync(new TdApi.CheckDatabaseEncryptionKey());
                break;

            case TdApi.Update.UpdateAuthorizationState { AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber }:
            case TdApi.Update.UpdateAuthorizationState { AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitCode }:
                _authNeeded = true;
                ReadyToAuthenticate.Set();
                break;

            case TdApi.Update.UpdateAuthorizationState { AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitPassword }:
                _authNeeded = true;
                _passwordNeeded = true;
                ReadyToAuthenticate.Set();
                break;

            case TdApi.Update.UpdateUser:
                ReadyToAuthenticate.Set();
                break;

            case TdApi.Update.UpdateConnectionState { State: TdApi.ConnectionState.ConnectionStateReady }:
                break;

            case TdApi.Update.UpdateNewMessage message:
                if (message.Message.Content is TdApi.MessageContent.MessageDocument document)
                {
                    await Client.DownloadFileAsync(document.Document.Document_.Id, 1);
                }
                break;

            case TdApi.Update.UpdateFile file:
                var status = file.File.Local.IsDownloadingCompleted;
                if (status)
                {
                    var path = file.File.Local.Path;
                    await _controller.LoadJson(path);
                }
                break;
        }
    }
    
    private static async Task<TdApi.User> GetCurrentUser()
    {
        return await Client.ExecuteAsync(new TdApi.GetMe());
    }

    private async Task SendMessageToBot(string text)
    {
        TdApi.InputMessageContent content = new TdApi.InputMessageContent.InputMessageText()
        {
            Extra = null,
            Text = new TdApi.FormattedText()
            {
                Extra = null,
                Text = text
            }
        };
            
        await Client.ExecuteAsync(new TdApi.SendMessage()
        {
            ChatId = long.Parse(_optionsMonitor.CurrentValue.ChatBotId),
            InputMessageContent = content
        });
    }
    
    private static async IAsyncEnumerable<TdApi.Chat> GetChannels(int limit)
    {
        var chats = await Client.ExecuteAsync(new TdApi.GetChats
        {
            Limit = limit
        });

        foreach (var chatId in chats.ChatIds)
        {
            var chat = await Client.ExecuteAsync(new TdApi.GetChat
            {
                ChatId = chatId
            });

            if (chat.Type is TdApi.ChatType.ChatTypeSupergroup or TdApi.ChatType.ChatTypeBasicGroup or TdApi.ChatType.ChatTypePrivate)
            {
                yield return chat;
            }
        }
    }

    private async Task GettingData()
    {
        while (true)
        {
            var now = DateTime.Now;
            var today = DateOnly.FromDateTime(now);
            
            await LoadJson();

            if (_updateDate.Institutes <= today)
            {
                await GetInstitutes();
            }
            if (_updateDate.Groups <= today)
            {
                await GetGroups();
            }
            if (_updateDate.Teachers <= today)
            {           
                await GetTeachers();
            }
            if (_updateDate.Disciplines <= today)
            {
                await GetDisciplines();
            }
            if (_updateDate.OtherDisciplines <= today)
            {
                await GetOtherDisciplines();
            }
            if (_updateDate.Classrooms <= today)
            {
                await GetClassrooms();
            }
            if (_updateDate.ScheduleThreeMonths <= today)
            {
                await GetScheduleThreeMonths();
            }
            if (_updateDate.ScheduleTwoWeeks <= today)
            {
                await GetScheduleTwoWeeks();
            }
            
            
            // To launch without a docker
            // await using (var createStream = File.Open($"{RootPath}\\getting-service\\updatedates.json", FileMode.Create))
            // {
            //     await JsonSerializer.SerializeAsync(createStream, _updateDate);
            // }
        
            // To launch with a docker
            await using (var createStream = File.Open($"{RootPath}updatedates.json", FileMode.Create))
            {
                await JsonSerializer.SerializeAsync(createStream, _updateDate);
            }

            var tomorrow = now.AddDays(1);
            var durationUntilMidnight = tomorrow.Date - now;
            await Task.Delay(durationUntilMidnight);
        }
    }

    private static async Task LoadJson()
    {
        // To launch without a docker
        // using var r = new StreamReader($"{RootPath}\\getting-service\\updatedates.json"); 
        
        // To launch with a docker
        using var r = new StreamReader($"{RootPath}updatedates.json");
        
        var json = await r.ReadToEndAsync();
        
        _updateDate = JsonConvert.DeserializeObject<UpdateDate>(json) ?? new UpdateDate();
    }

    private async Task GetInstitutes()
    {
        await SendMessageToBot("/api/institutes");
        _updateDate.Institutes = DateOnly.FromDateTime(DateTime.Now).AddMonths(1);
    }

    private async Task GetGroups()
    {
        await SendMessageToBot("/api/groups");
        _updateDate.Groups = DateOnly.FromDateTime(DateTime.Now).AddMonths(1);
    }
    
    private async Task GetTeachers()
    {
        await SendMessageToBot("/api/teachers");
        _updateDate.Teachers = DateOnly.FromDateTime(DateTime.Now).AddMonths(1);
    }
    
    private async Task GetDisciplines()
    {
        await SendMessageToBot("/api/disciplines");
        _updateDate.Disciplines = DateOnly.FromDateTime(DateTime.Now).AddMonths(1);
    }
    
    private async Task GetOtherDisciplines()
    {
        await SendMessageToBot("/api/other_disciplines");
        _updateDate.OtherDisciplines = DateOnly.FromDateTime(DateTime.Now).AddMonths(1);
    }
    
    private async Task GetClassrooms()
    {
        await SendMessageToBot("/api/classrooms");
        _updateDate.Classrooms = DateOnly.FromDateTime(DateTime.Now).AddMonths(1);
    }
    
    private async Task GetScheduleTwoWeeks()
    {
        await SendMessageToBot("/api/schedule/two_weeks");
        _updateDate.ScheduleTwoWeeks = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
    }
    
    private async Task GetScheduleThreeMonths()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        await SendMessageToBot($"/api/schedule/month?date={date.Year}-{date.Month}");
        date = date.AddMonths(1);
        await SendMessageToBot($"/api/schedule/month?date={date.Year}-{date.Month}");
        date = date.AddMonths(1);
        await SendMessageToBot($"/api/schedule/month?date={date.Year}-{date.Month}");
        _updateDate.ScheduleThreeMonths = DateOnly.FromDateTime(DateTime.Now).AddMonths(1);
    }
}