using TeleWoL.Settings;

namespace TeleWoL.WakeOnLan;

internal interface IWoLSender : IDisposable
{
    Task Wake(MacAddress mac);
}
