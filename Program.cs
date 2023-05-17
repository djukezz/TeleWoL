using StateTest;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Collections.Concurrent;
using TeleWoL.Settings;

ConcurrentDictionary<long, Session> _sessions = new ConcurrentDictionary<long, Session>();
var settingsWrapper = new SettingsWrapper<GlobalSettings>("settings.bin");

var botClient = new TelegramBotClient("6050754775:AAFQUT_kLk8uzvvDDbaIk6hnDWJrkvTY-Mw");

using CancellationTokenSource cts = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>(),
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

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
        settingsWrapper.Save();
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
        id => new Session(settingsWrapper.Settings, new UserContext { UserId = id }));

Console.ReadLine();

cts.Cancel();