using System.Net.Sockets;
using System.Net;
using TeleWoL.Settings;

namespace TeleWoL.WakeOnLan;

internal sealed class SingleNicWoLSender : IWoLSender
{
    public IPEndPoint _localEp;
    public IPEndPoint _remoteEp;
    private UdpClient? _udpClient;
    private readonly object _lock = new object();

    public SingleNicWoLSender(IPAddress local, IPAddress remote)
    {
        _localEp = new IPEndPoint(local, 0);
        _remoteEp = new IPEndPoint(remote, 9);
    }

    public async Task Wake(MacAddress mac)
    {
        var data = MagicPacketBuilder.GetMagicPacket(mac);
        lock (_lock)
        {
            if (_udpClient == null)
                _udpClient = new(_localEp);
        }
        await _udpClient.SendAsync(data, _remoteEp);
    }

    public void Dispose()
    {
        _udpClient?.Dispose();
        _udpClient = null;
    }
}
