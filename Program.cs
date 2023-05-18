using StateTest;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Collections.Concurrent;
using TeleWoL.Settings;

const string _settingsFile = "settings.json";
ConcurrentDictionary<long, Session> _sessions = new ConcurrentDictionary<long, Session>();
using var _settingsWrapper = new SettingsWrapper<GlobalSettings>("settings.json");

if (string.IsNullOrWhiteSpace(_settingsWrapper.Settings.Token))
    throw new Exception($"Token must be specified in {_settingsFile}");

Console.WriteLine($"Admin ids can be specified by parameters");
Console.WriteLine($"Press any key to terminate app");

AddAdminsFromArgs(_settingsWrapper.Settings, Environment.GetCommandLineArgs().Skip(1));
_settingsWrapper.Save();

var botClient = new TelegramBotClient(_settingsWrapper.Settings.Token);

using CancellationTokenSource cts = new();

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: new()
    {
        AllowedUpdates = Array.Empty<UpdateType>(),
    },
    cancellationToken: cts.Token
);

Console.ReadKey();

cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
{
    if (update.CallbackQuery is { } query)
    {
        await ProcessQuery(bot, query, ct);
        return;
    }

    if (update.Message is { } message)
    {
        await ProcessMessage(bot, message, ct);
        return;
    }
}

async Task ProcessMessage(ITelegramBotClient bot, Message message, CancellationToken ct)
{
    if (message.Text is not { } messageText)
        return;

    var user = message.From;
    if (user == null)
        return;

    var chatId = message.Chat.Id;

    var session = GetSession(user.Id);

    if (messageText == string.Empty)
        return;

    await Process(bot, ct, chatId, session, messageText);
}

async Task ProcessQuery(ITelegramBotClient bot, CallbackQuery query, CancellationToken ct)
{
    var session = GetSession(query.From.Id);
    if (string.IsNullOrEmpty(query.Data))
        return;

    if (query.Message?.Chat.Id is long chatId)
        await Process(bot, ct, chatId, session, query.Data);
}

async Task Process(ITelegramBotClient bot, CancellationToken ct, long chatId,
    Session session, string command)
{
    var responses = session.Execute(command);
    if (session.GetIsSettingsChanged())
        _settingsWrapper.Save();
    foreach (var response in responses)
    {
        await bot.SendTextMessageAsync(
            chatId: chatId,
            text: response.Text == string.Empty ? "Select" : response.Text,
            replyMarkup: response.KeyboardMarkup,
            cancellationToken: ct);
    }        
}

Task HandlePollingErrorAsync(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
{
    return Task.CompletedTask;
}

Session GetSession(long userId) => _sessions.GetOrAdd(userId,
        id => new Session(_settingsWrapper.Settings, new UserContext { UserId = id }));

static void AddAdminsFromArgs(GlobalSettings settings, IEnumerable<string> ids)
{
    foreach (var id in ids)
    {
        if (!long.TryParse(id, out long userId))
            continue;
        var userSettings = settings.GetOrAdd(userId);
        userSettings.Permission = UserPermission.Admin;
    }
}