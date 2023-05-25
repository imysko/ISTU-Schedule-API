using getting_service.Data;
using getting_service.Data.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TdLib;

namespace getting_service.Services;

public class TelegramService : BackgroundService
{
    private readonly IOptionsMonitor<TelegramConfig> _optionsMonitor;

    private TdClient _client = null!;
    private AuthorizationState _authorizationState;

    public TelegramService(IOptionsMonitor<TelegramConfig> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
        PrintConfig();
    }
    
    private void PrintConfig()
    {
        Console.WriteLine($"Api Id: {_optionsMonitor.CurrentValue.ApiId}");
        Console.WriteLine($"Api hash: {_optionsMonitor.CurrentValue.ApiHash}");
        Console.WriteLine($"Phone number: {_optionsMonitor.CurrentValue.PhoneNumber}");
        Console.WriteLine($"ChatBot Id: {_optionsMonitor.CurrentValue.ChatBotId}");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Starting telegram service");
        _client = new TdClient();
        _client.Bindings.SetLogVerbosityLevel(1);
        _client.UpdateReceived += async (_, update) => { await ProcessUpdates(update); };
        while (!stoppingToken.IsCancellationRequested)
        {
            while (Program.MessageQueue.TryDequeue(out var message))
            {
                await HandleUserMessage(message);
            }

            await Task.Delay(TimeSpan.FromMilliseconds(100), stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Stopping telegram service");
        await base.StopAsync(stoppingToken);
    }

    private async Task ProcessUpdates(TdApi.Update update)
    {
        switch (update)
        {
            case TdApi.Update.UpdateAuthorizationState
            {
                AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters
            }:
                var filesLocation = Path.Combine(AppContext.BaseDirectory, "db");
                await _client.ExecuteAsync(new TdApi.SetTdlibParameters
                {
                    Parameters = new TdApi.TdlibParameters
                    {
                        ApiId = int.Parse(_optionsMonitor.CurrentValue.ApiId),
                        ApiHash = _optionsMonitor.CurrentValue.ApiHash,
                        DeviceModel = "Linux Server",
                        SystemLanguageCode = "en",
                        ApplicationVersion = "1.0.0",
                        DatabaseDirectory = filesLocation,
                        FilesDirectory = filesLocation,
                    }
                });
                break;

            case TdApi.Update.UpdateAuthorizationState
            {
                AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitEncryptionKey
            }:
                await _client.ExecuteAsync(new TdApi.CheckDatabaseEncryptionKey());
                break;

            case TdApi.Update.UpdateAuthorizationState
            {
                AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber
            }:
                await _client.ExecuteAsync(new TdApi.SetAuthenticationPhoneNumber
                {
                    PhoneNumber = _optionsMonitor.CurrentValue.PhoneNumber
                });
                break;

            case TdApi.Update.UpdateAuthorizationState
            {
                AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitCode
            }:
                _authorizationState = AuthorizationState.WaitCode;
                break;

            case TdApi.Update.UpdateAuthorizationState
            {
                AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitPassword
            }:
                _authorizationState = AuthorizationState.WaitPassword;
                break;

            case TdApi.Update.UpdateAuthorizationState
            {
                AuthorizationState: TdApi.AuthorizationState.AuthorizationStateReady
            }:
                _authorizationState = AuthorizationState.Authorized;
                await GetAndPrintUserInfo();
                break;

            case TdApi.Update.UpdateNewMessage message:
                if (message.Message.Content is not TdApi.MessageContent.MessageDocument document) return;
                await _client.DownloadFileAsync(document.Document.Document_.Id, 1);
                break;

            case TdApi.Update.UpdateFile file:
                var status = file.File.Local.IsDownloadingCompleted;
                if (!status) return;
                var path = file.File.Local.Path;
                Program.FilePathQueue.Enqueue(path);
                break;
        }
    }

    private async Task HandleUserMessage(string message)
    {
        if (_authorizationState != AuthorizationState.Authorized)
        {
            await HandleUserAuthenticationMessage(message);
            return;
        }
        switch (message)
        {
            case "all":
                await SendMessageToBot("/api/institutes");
                await SendMessageToBot("/api/groups");
                await SendMessageToBot("/api/teachers");
                await SendMessageToBot("/api/classrooms");
                await SendMessageToBot("/api/disciplines");
                await SendMessageToBot("/api/lessons_time");
                await SendMessageToBot("/api/other_disciplines");
                await SendMessageToBot("/api/queries");
                Thread.Sleep((int)TimeSpan.FromSeconds(10).TotalMilliseconds);
                await SendMessageToBot("/api/schedule/two_weeks");
                break;
            case "lessons time":
                await SendMessageToBot("/api/lessons_time");
                break;
            case "institutes":
                await SendMessageToBot("/api/institutes");
                break;
            case "groups":
                await SendMessageToBot("/api/groups");
                break;
            case "teachers":
                await SendMessageToBot("/api/teachers");
                break;
            case "classrooms":
                await SendMessageToBot("/api/classrooms");
                break;
            case "disciplines":
                await SendMessageToBot("/api/disciplines");
                break;
            case "other disciplines":
                await SendMessageToBot("/api/other_disciplines");
                break;
            case "queries":
                await SendMessageToBot("/api/queries");
                break;
            case "schedule two weeks":
                await SendMessageToBot("/api/schedule/two_weeks");
                break;
            case "schedule three months":
                var date = DateOnly.FromDateTime(DateTime.Now);
                await SendMessageToBot($"/api/schedule/month?date={date.Year}-{date.Month}");
                date = date.AddMonths(1);
                await SendMessageToBot($"/api/schedule/month?date={date.Year}-{date.Month}");
                date = date.AddMonths(1);
                await SendMessageToBot($"/api/schedule/month?date={date.Year}-{date.Month}");
                break;
        }
    }

    private async Task HandleUserAuthenticationMessage(string message)
    {
        switch (_authorizationState)
        {
            case AuthorizationState.WaitCode:
                await _client.ExecuteAsync(new TdApi.CheckAuthenticationCode
                {
                    Code = message
                });
                break;
            case AuthorizationState.WaitPassword:
                await _client.ExecuteAsync(new TdApi.CheckAuthenticationPassword
                {
                    Password = message
                });
                break;
            case AuthorizationState.Unauthorized:
                break;
            case AuthorizationState.WaitPhone:
                break;
            case AuthorizationState.Authorized:
                break;
        }
    }

    private async Task GetAndPrintUserInfo()
    {
        const int channelLimit = 5;
        var currentUser = await GetCurrentUser();
        var fullUserName = $"{currentUser.FirstName} {currentUser.LastName}".Trim();
        Console.WriteLine(
            $"Successfully logged in as [{currentUser.Id}] / [@{currentUser.Username}] / [{fullUserName}]");
        var channels = GetChannels(channelLimit);
        Console.WriteLine($"Top {channelLimit} channels:");
        await foreach (var channel in channels)
        {
            Console.WriteLine($"[{channel.Id}] -> [{channel.Title}]");
        }
    }

    private async Task<TdApi.User> GetCurrentUser()
    {
        return await _client.ExecuteAsync(new TdApi.GetMe());
    }

    private async IAsyncEnumerable<TdApi.Chat> GetChannels(int limit)
    {
        var chats = await _client.ExecuteAsync(new TdApi.GetChats
        {
            Limit = limit
        });

        foreach (var chatId in chats.ChatIds)
        {
            var chat = await _client.ExecuteAsync(new TdApi.GetChat
            {
                ChatId = chatId
            });

            if (chat.Type is TdApi.ChatType.ChatTypeSupergroup or TdApi.ChatType.ChatTypeBasicGroup
                or TdApi.ChatType.ChatTypePrivate)
            {
                yield return chat;
            }
        }
    }

    private async Task SendMessageToBot(string text)
    {
        TdApi.InputMessageContent content = new TdApi.InputMessageContent.InputMessageText()
        {
            Extra = null,
            Text = new TdApi.FormattedText
            {
                Extra = null,
                Text = text
            }
        };

        await _client.ExecuteAsync(new TdApi.SendMessage()
        {
            ChatId = long.Parse(_optionsMonitor.CurrentValue.ChatBotId),
            InputMessageContent = content
        });
    }
}