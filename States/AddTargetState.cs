using StateTest;
using TeleWoL.Commands;
using TeleWoL.Settings;

namespace TeleWoL.States;

internal class AddTargetState : UserStateBase
{
    public AddTargetState(UserSettings settings) : base(settings)
    {
        _commands.Add(GoHomeCommand.Instance);
    }

    private string _name = string.Empty;
    private bool _macEntering;
    private static readonly Dictionary<char, byte> _validChars = Enumerable.Range(0, 16).ToDictionary(v => v.ToString("X").ToCharArray().Single(), v => (byte)v);

    public override string Description => _macEntering ? $"Enter MAC for '{_name}'" : "Enter target name";

    protected override Response? Execute(CommandBase command, out StateBase newState)
    {
        newState = this;
        if (command is GoHomeCommand)
        {
            newState = new MainState(_settings);
            return null;
        }
        if (command is StringCommand stringCommand)
        {
            string text = stringCommand.Text.Trim();
            if (text != string.Empty)
            {
                if (!_macEntering && ValidateName(text))
                {
                    _name = text;
                    _macEntering = true;
                    return null;
                }
                else if (_macEntering && ValidateMac(text, out var mac))
                {
                    var target = new Target { Name = _name, Mac = mac };
                    _settings.AddTarget(target);
                    newState = new SettingsState(_settings);
                    return new Response($"Target {target.Name} added!");
                }
            }
            return new Response("Wrong format");
        }

        return Response.Unknown;
    }

    private bool ValidateMac(string text, out MacAddress mac) => MacAddress.TryParse(FromHex(text), out mac);

    private static IEnumerable<byte> FromHex(string chars)
    {
        int prev = 0;
        int count = 0;
        foreach (char c in chars.ToUpper())
        {
            if (!_validChars.TryGetValue(c, out byte b))
                continue;
            prev = prev << 4;
            prev |= b & 0x0F;
            count++;
            if (count % 2 == 0)
                yield return (byte)prev;
        }
    }


    private bool ValidateName(string text) => !_settings.GetTargets().Any(t => t.Name == text);

    protected override CommandBase? ConvertCommand(string command) => StringCommand.FromString(command);
}
