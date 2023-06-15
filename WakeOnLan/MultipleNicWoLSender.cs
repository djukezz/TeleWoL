using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using TeleWoL.Settings;
using System.Collections.Concurrent;

namespace TeleWoL.WakeOnLan;

internal sealed class MultipleNicWoLSender : IWoLSender, IDisposable
{
    private ConcurrentDictionary<MacAddress, byte[]> _cache = new ConcurrentDictionary<MacAddress, byte[]>();
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
        byte[] magicPacket = _cache.GetOrAdd(mac, m => MagicPacketBuilder.BuildMagicPacket(mac));
        await Wake(magicPacket);
    }

    public async Task Wake(byte[] mac)
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
            foreach (MulticastIPAddressInformation multicastIPAddressInformation in iPInterfaceProperties.MulticastAddresses)
            {
                IPAddress multicastIpAddress = multicastIPAddressInformation.Address;
                if (multicastIpAddress.ToString().StartsWith("ff02::1%", StringComparison.OrdinalIgnoreCase)) // Ipv6: All hosts on LAN (with zone index)
                {
                    UnicastIPAddressInformation? unicastIPAddressInformation = iPInterfaceProperties.UnicastAddresses.Where((u) =>
                        u.Address.AddressFamily == AddressFamily.InterNetworkV6 && !u.Address.IsIPv6LinkLocal).FirstOrDefault();
                    if (unicastIPAddressInformation != null)
                    {
                        yield return (unicastIPAddressInformation.Address, multicastIpAddress);
                    }
                }
                else if (multicastIpAddress.ToString().Equals("224.0.0.1")) // Ipv4: All hosts on LAN
                {
                    UnicastIPAddressInformation? unicastIPAddressInformation = iPInterfaceProperties.UnicastAddresses.Where((u) =>
                        u.Address.AddressFamily == AddressFamily.InterNetwork && !iPInterfaceProperties.GetIPv4Properties().IsAutomaticPrivateAddressingActive).FirstOrDefault();
                    if (unicastIPAddressInformation != null)
                    {
                        yield return (unicastIPAddressInformation.Address, multicastIpAddress);
                    }
                }
            }
        }
    }

    public void Dispose()
    {
        foreach (var sender in _senders)
            sender.Dispose();
        _senders.Clear();
    }
}
