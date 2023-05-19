using TeleWoL.Settings;

namespace TeleWoL.WakeOnLan;

internal static class MagicPacketBuilder
{
    public static byte[] BuildMagicPacket(MacAddress mac)
    {
        IEnumerable<byte> header = Enumerable.Repeat((byte)0xff, 6);
        IEnumerable<byte> data = Enumerable.Repeat(mac.GetBytes(), 16).SelectMany(m => m);
        return header.Concat(data).ToArray();
    }
}
