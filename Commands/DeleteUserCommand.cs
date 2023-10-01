using TeleWoL.Settings;

namespace TeleWoL.Commands;

internal sealed class DeleteUserCommand : CommandBase
{
    public DeleteUserCommand(GlobalSettings settings, UserSettings user)
    {
        _settings = settings;
        _user = user;

        string name = user.ToString();
        Text = name;
        Id = $"delete_{_user.UserId}";
    }

    public Response Execute()
    {
        _settings.Delete(_user.UserId);
        return $"User '{Text} ' deleted";
    }

    private readonly GlobalSettings _settings;
    private readonly UserSettings _user;
}