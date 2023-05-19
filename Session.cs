using LaserControl.IoC;
using Ninject;
using Ninject.Extensions.ChildKernel;
using Telegram.Bot.Types.ReplyMarkups;
using TeleWoL.IoC;
using TeleWoL.Settings;
using TeleWoL.States;

namespace StateTest;

internal class Session
{
    private bool _isSettingsChanged;
    private StateBase? _state;
    public UserContext UserContext { get; }

    public Session(IKernel parentKernel, UserContext userContext)
    {
        StandardKernel kernel = new ChildKernel(parentKernel,
            new StatesModule(), new UserSettingsModule(userContext));

        UserContext = userContext;
        var statesFactory = kernel.Get<StatesFactory>();
        var userSettings = kernel.Get<UserSettings>();

        StateBase state = userSettings.Permission == UserPermission.None ?
            statesFactory.Create<LoginState>() :
            statesFactory.Create<MainState>();
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
