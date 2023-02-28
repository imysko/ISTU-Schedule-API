using getting_service.DataBase.Context;
using TdLib.Bindings;
using TdLib;
using getting_service.DataBase.Controllers;
using Microsoft.Extensions.Configuration;

namespace getting_service;

internal static class Program
{
    private const string ApplicationVersion = "1.0.0";
    
    private static readonly int ApiId;
    private static readonly string ApiHash;

    private static readonly string PhoneNumber;

    private static readonly long ChatBotId;

    private static readonly TdClient Client;
    private static readonly ManualResetEventSlim ReadyToAuthenticate = new();

    private static bool _authNeeded;
    private static bool _passwordNeeded;

    private static readonly ScheduleDbController Controller;

    static Program()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json", true, true);
        var config = builder.Build();

        ApiId = Convert.ToInt32(config["ConnectionStrings:TelegramApiId"]);
        ApiHash = config["ConnectionStrings:TelegramApiHash"]!;
        PhoneNumber = config["ConnectionStrings:TelegramPhoneNumber"]!;

        ChatBotId = Convert.ToInt64(config["ConnectionStrings:ChatBotId"]);

        var context = new ScheduleDbContext(config["ConnectionStrings:ScheduleDB"]!);
        Controller = new ScheduleDbController(context);
        
        Client = new TdClient();
    }
    
    private static async Task Main()
    {
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
        
        Console.WriteLine("Press ENTER to exit from application");
        Console.ReadLine();
    }

    private static async Task HandleAuthentication()
    {
        await Client.ExecuteAsync(new TdApi.SetAuthenticationPhoneNumber
        {
            PhoneNumber = PhoneNumber
        });

        Console.Write("Insert the login code: ");
        var code = Console.ReadLine();

        await Client.ExecuteAsync(new TdApi.CheckAuthenticationCode
        {
            Code = code
        });

        if(!_passwordNeeded) { return; }

        Console.Write("Insert the password: ");
        var password = Console.ReadLine();

        await Client.ExecuteAsync(new TdApi.CheckAuthenticationPassword
        {
            Password = password
        });
    }

    private static async Task ProcessUpdates(TdApi.Update update)
    {
        switch (update)
        {
            case TdApi.Update.UpdateAuthorizationState { AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters }:
                var filesLocation = Path.Combine(AppContext.BaseDirectory, "db");
                await Client.ExecuteAsync(new TdApi.SetTdlibParameters
                {
                    Parameters = new TdApi.TdlibParameters
                    {
                        ApiId = ApiId,
                        ApiHash = ApiHash,
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
                    Console.WriteLine(document.Document);
                    await Client.DownloadFileAsync(document.Document.Document_.Id, 1);
                }
                break;

            case TdApi.Update.UpdateFile file:
                var status = file.File.Local.IsDownloadingCompleted;
                if (status)
                {
                    var path = file.File.Local.Path;
                    await Controller.LoadJson(path);
                }
                break;
        }
    }
    
    private static async Task<TdApi.User> GetCurrentUser()
    {
        return await Client.ExecuteAsync(new TdApi.GetMe());
    }

    private static async Task SendMessageToBot(string text)
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
            ChatId = ChatBotId,
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
}