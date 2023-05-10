using StateTest;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Collections.Concurrent;

ConcurrentDictionary<long, Session> _sessions = new ConcurrentDictionary<long, Session>();
UsersSettings _usersSettings = new UsersSettings();

var botClient = new TelegramBotClient("6050754775:AAFQUT_kLk8uzvvDDbaIk6hnDWJrkvTY-Mw");

using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
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
    foreach (var response in responses)
    {
        Message sentMessage = await bot.SendTextMessageAsync(
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
        id => new Session(_usersSettings, new UserContext { UserId = id }));

Console.ReadLine();

cts.Cancel();