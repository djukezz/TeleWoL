using Telegram.Bot.Types.ReplyMarkups;

namespace StateTest;

internal class Session
{
    private StateBase _state = new LoginState();
    public UserContext UserContext { get; }

    public Session(UsersSettings settings, UserContext userContext)
    {
        UserContext = userContext;

        var s = settings.Users.FirstOrDefault(u => u.UserId == userContext.UserId);
        ChangeState(s != null ? new IdleState(s) : _state);
    }

    public string StateName => _state.Name;
    public IEnumerable<CommandBase> Commands => _state.Commands;

    public IEnumerable<Response> Execute(string command)
    {
        var msg = _state.Execute(command, out var newState);
        if (msg != null)
            yield return msg;

        if (ReferenceEquals(newState, _state))
            yield break;

        yield return ChangeState(newState);
    }

    private Response ChangeState(StateBase state)
    {
        _state = state;

        var buttons = state.Commands
            .Where(c => c.Id != string.Empty)
            .Select(c => new[] { InlineKeyboardButton.WithCallbackData(c.Text, c.Id) })
            .ToList();
        Response commandsResponse = new Response(state.Description,
            buttons.Count == 0 ? null : new InlineKeyboardMarkup(buttons));
        return commandsResponse;
    }
}
