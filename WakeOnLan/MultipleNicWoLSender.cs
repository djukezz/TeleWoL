using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using TeleWoL.Settings;
using System.Collections.Concurrent;

namespace TeleWoL.WakeOnLan;

internal sealed class MultipleNicWoLSender : IWoLSender, IDisposable
{
    private List<SingleNicWoLSender> _senders;

    public MultipleNicWoLSender(Func<IPAddress, bool>? selector = null)
    {
        _senders = GetAllIPs()
            .Where(p => selector?.Invoke(p.local) ?? true)
            .Select(p => new SingleNicWoLSender(p.local, p.remote))
            .ToList();
    }

    public async Task Wake(MacAddress mac)
    {
        foreach (var sender in _senders)
        {
            await sender.Wake(mac);
        }
    }

    private static IEnumerable<(IPAddress local, IPAddress remote)> GetAllIPs()
    {
        foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces().Where((n) =>
            n.NetworkInterfaceType != NetworkInterfaceType.Loopback && n.OperationalStatus == OperationalStatus.Up))
        {
            IPInterfaceProperties iPInterfaceProperties = networkInterface.GetIPProperties();
            foreach (var info in iPInterfaceProperties.UnicastAddresses)
            {
                if (info.Address.AddressFamily != AddressFamily.InterNetwork)
                    continue;
                yield return (info.Address, IPAddress.Broadcast);
            }
            yield break;
        }
    }

    public void Dispose()
    {
        foreach (var sender in _senders)
            sender.Dispose();
        _senders.Clear();
    }
}
