using Telegram.Bot.Types.ReplyMarkups;
using TeleWoL.Settings;
using TeleWoL.States;

namespace StateTest;

internal class Session
{
    private bool _isSettingsChanged;
    private StateBase? _state;
    public UserContext UserContext { get; }

    public Session(GlobalSettings settings, UserContext userContext)
    {
        UserContext = userContext;

        var s = settings.Get(userContext.UserId);
        StateBase state = s == null ? new LoginState(settings, userContext) : new MainState(s);
        ChangeState(state);
    }

    public IEnumerable<Response> Execute(string command)
    {
        if (_state == null)
            yield break;

        var msg = _state.Execute(command, out var newState);
        if (msg != null)
            yield return msg;

        yield return ChangeState(newState);
    }

    public bool GetIsSettingsChanged()
    {
        bool isSettingsChanged = _isSettingsChanged;
        _isSettingsChanged = false;
        return isSettingsChanged;
    }

    private Response ChangeState(StateBase state)
    {
        if (_state != null)
            _state.SettingsChanged -= SettingsChanged;
        _state = state;
        if (_state != null)
            _state.SettingsChanged += SettingsChanged;

        var buttons = state.Commands
            .Where(c => c.Id != string.Empty)
            .Select(c => new[] { InlineKeyboardButton.WithCallbackData(c.Text, c.Id) })
            .ToList();
        Response commandsResponse = new Response(state.Description,
            buttons.Count == 0 ? null : new InlineKeyboardMarkup(buttons));

        return commandsResponse;
    }

    private void SettingsChanged() => _isSettingsChanged = true;
}
