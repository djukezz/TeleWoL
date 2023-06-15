using System.Collections.Concurrent;
using TeleWoL.Settings;

namespace TeleWoL.WakeOnLan;

internal static class MagicPacketBuilder
{
    private static readonly ConcurrentDictionary<MacAddress, ReadOnlyMemory<byte>> _cache = new();

    public static ReadOnlyMemory<byte> GetMagicPacket(MacAddress mac)
    {
        return _cache.GetOrAdd(mac, m => BuildMagicPacket(m));
    }

    private static byte[] BuildMagicPacket(MacAddress mac)
    {
        IEnumerable<byte> header = Enumerable.Repeat((byte)0xff, 6);
        IEnumerable<byte> data = Enumerable.Repeat(mac.GetBytes(), 16).SelectMany(m => m);
        return header.Concat(data).ToArray();
    }
}
