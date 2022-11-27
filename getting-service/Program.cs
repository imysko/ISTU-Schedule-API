using TdLib.Bindings;
using TdLib;

namespace getting_service;

internal static class Program
{
    private const string ApplicationVersion = "1.0.0";
    
    private static readonly int ApiId = Int32.Parse(Environment.GetEnvironmentVariable("ApiId")!);
    private static readonly string ApiHash = Environment.GetEnvironmentVariable("ApiHash")!;
    
    private static readonly string PhoneNumber = Environment.GetEnvironmentVariable("PhoneNumber")!;

    private static TdClient _client;
    private static readonly ManualResetEventSlim ReadyToAuthenticate = new();

    private static bool _authNeeded;
    private static bool _passwordNeeded;

    private static async Task Main()
    {
        _client = new TdClient();
        _client.Bindings.SetLogVerbosityLevel(TdLogLevel.Fatal);

        _client.UpdateReceived += async (_, update) => { await ProcessUpdates(update); };

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

        await SendMessageToBot("/api/institutes");
        
        Console.WriteLine("Press ENTER to exit from application");
        Console.ReadLine();
    }

    private static async Task HandleAuthentication()
    {
        await _client.ExecuteAsync(new TdApi.SetAuthenticationPhoneNumber
        {
            PhoneNumber = PhoneNumber
        });

        Console.Write("Insert the login code: ");
        var code = Console.ReadLine();

        await _client.ExecuteAsync(new TdApi.CheckAuthenticationCode
        {
            Code = code
        });

        if(!_passwordNeeded) { return; }

        Console.Write("Insert the password: ");
        var password = Console.ReadLine();

        await _client.ExecuteAsync(new TdApi.CheckAuthenticationPassword
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
                await _client.ExecuteAsync(new TdApi.SetTdlibParameters
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
                await _client.ExecuteAsync(new TdApi.CheckDatabaseEncryptionKey());
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
                if (message?.Message.Content is TdApi.MessageContent.MessageDocument)
                {
                    var content = message?.Message.Content;
                    if (content is TdApi.MessageContent.MessageDocument)
                    {
                        var document = content as TdApi.MessageContent.MessageDocument;
                        Console.WriteLine(document?.Document);
                        if (document != null)
                        {
                            await TdApi.DownloadFileAsync(_client, document.Document.Document_.Id, 1);
                        }
                    }
                }
                break;

            case TdApi.Update.UpdateFile file:
                var status = file.File.Local.IsDownloadingCompleted;
                if (status)
                {
                    var path = file.File.Local.Path;
                    switch(path)
                    {
                        case var _ when path.Contains("institutes"): 
                            await SaveInstitutes(path);
                            break;
                        case var _ when path.Contains("groups"): 
                            await SaveGroups(path);
                            break;
                        case var _ when path.Contains("lessons_time"): 
                            await SaveLessonsTime(path);
                            break;
                        case var _ when path.Contains("teachers"): 
                            await SaveTeachers(path);
                            break;
                        case var _ when path.Contains("lessons_names"): 
                            await SaveLessonsNames(path);
                            break;
                        case var _ when path.Contains("classrooms"): 
                            await SaveClassrooms(path);
                            break;
                        case var _ when path.Contains("month"): 
                            await SaveSchedule(path);
                            break;
                        case var _ when path.Contains("two_weeks"): 
                            await SaveSchedule(path);
                            break;
                    }
                }
                break;
            
            default:
                // ReSharper disable once EmptyStatement
                ;
                break;
        }
    }
    
    private static async Task SaveInstitutes(string path)
    {
        
    }
    
    private static async Task SaveGroups(string path)
    {
        
    }

    private static async Task SaveLessonsTime(string path)
    {
        
    }
    
    private static async Task SaveTeachers(string path)
    {
        
    }
    
    private static async Task SaveLessonsNames(string path)
    {
        
    }
    
    private static async Task SaveClassrooms(string path)
    {
        
    }
    
    private static async Task SaveSchedule(string path)
    {
        
    }

    private static async Task<TdApi.User> GetCurrentUser()
    {
        return await _client.ExecuteAsync(new TdApi.GetMe());
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
            
        await _client.ExecuteAsync(new TdApi.SendMessage()
        {
            ChatId = 5739830666,
            InputMessageContent = content
        });
    }
    
    private static async IAsyncEnumerable<TdApi.Chat> GetChannels(int limit)
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

            if (chat.Type is TdApi.ChatType.ChatTypeSupergroup or TdApi.ChatType.ChatTypeBasicGroup or TdApi.ChatType.ChatTypePrivate)
            {
                yield return chat;
            }
        }
    }
}