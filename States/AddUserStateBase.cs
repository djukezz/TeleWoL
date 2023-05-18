using StateTest;
using TeleWoL.Commands;
using TeleWoL.Settings;

namespace TeleWoL.States;

internal abstract class AddUserStateBase : StateBase
{
    public override void Init()
    {
        _commands.Add(GoHomeCommand.Instance);
    }

    protected abstract UserPermission GetPermission();

    public override string Description => "Enter user Id or add contact";

    protected override Response? Execute(CommandBase command, out StateBase newState)
    {
        newState = this;
        if (command is IStateCommand stateCommand)
        {
            newState = stateCommand.Execute(StatesFactory);
            return null;
        }

        if (command is StringCommand stringCommand)
        {
            if (long.TryParse(stringCommand.Text, out long userId))
            {
                var userSettings = GlobalSettings.GetOrAdd(userId);
                var permission = UserPermissionEx.Max(userSettings.Permission, GetPermission());
                userSettings.Permission = permission;
                FireSettingsChanged();
                return $"{permission} '{userId}' added";
            }
        }

        return Response.Unknown;
    }

    protected override CommandBase? ConvertCommand(string command) => StringCommand.FromString(command);
}
