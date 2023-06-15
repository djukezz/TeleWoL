using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Collections.Concurrent;
using TeleWoL.Settings;
using Ninject;
using TeleWoL.IoC;

namespace TeleWoL;

internal sealed class Bot : IDisposable
{
    private ConcurrentDictionary<long, Session> _sessions = new ConcurrentDictionary<long, Session>();
    private readonly StandardKernel _kernel;
    private GlobalSettings _settings;
    private CancellationTokenSource _cts;

    public Bot()
    {
        _kernel = new StandardKernel(new SettingsModule(), new WoLModule());
        _settings = _kernel.Get<GlobalSettings>();
        _cts = new CancellationTokenSource();
    }

    public void AddAdmins(IEnumerable<long> ids) =>
        AddUsers(ids, UserPermission.Admin);

    public void AddUsers(IEnumerable<long> ids) =>
        AddUsers(ids, UserPermission.User);

    public async Task<User> Start()
    {
        if (string.IsNullOrWhiteSpace(_settings.Token))
            throw new Exception($"Token must be specified in settings file");

        var botClient = new TelegramBotClient(_settings.Token);

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: new()
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
            },
            cancellationToken: _cts.Token
        );

        return await botClient.GetMeAsync();
    }

    public void Stop()
    {
        _cts.Cancel();
    }

    private void AddUsers(IEnumerable<long> ids, UserPermission permission)
    {
        foreach (var id in ids)
        {
            var userSettings = _settings.GetOrAdd(id);
            userSettings.Permission = UserPermissionEx.Max(userSettings.Permission, permission);
        }
    }

    private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
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

    private async Task ProcessMessage(ITelegramBotClient bot, Message message, CancellationToken ct)
    {
        string messageText = message.Text ?? message.Contact?.UserId?.ToString() ?? string.Empty;

        var user = message.From;
        if (user == null)
            return;

        var chatId = message.Chat.Id;

        var session = GetSession(user);

        if (messageText == string.Empty)
            return;

        await Process(bot, ct, chatId, session, messageText);
    }

    private async Task ProcessQuery(ITelegramBotClient bot, CallbackQuery query, CancellationToken ct)
    {
        var session = GetSession(query.From);
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
            await bot.SendTextMessageAsync(
                chatId: chatId,
                text: response.Text == string.Empty ? "Select" : response.Text,
                replyMarkup: response.KeyboardMarkup,
                cancellationToken: ct);
        }
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        return Task.CompletedTask;
    }

    Session GetSession(User user)
    {
        return _sessions.GetOrAdd(user.Id,
            id => new Session(_kernel, new UserContext(user)));
    }

    public void Dispose()
    {
        Stop();
        _cts.Dispose();
    }
}