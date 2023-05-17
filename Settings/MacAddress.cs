using Newtonsoft.Json;

namespace TeleWoL.Settings
{
    [JsonObject(MemberSerialization.OptIn)]
    internal struct MacAddress
    {
        private MacAddress(ulong mac) => RawValue = mac;

        public MacAddress(IEnumerable<byte> bytes)
        {
            RawValue = Convert(bytes, out int count);
            if (count != _size)
                throw new ArgumentException();
        }

        private const int _size = 6;
        [JsonProperty]
        public ulong RawValue { get; private set; }

        public static bool TryParse(IEnumerable<byte> bytes, out MacAddress mac)
        {
            ulong address = Convert(bytes, out int count);
            if (count != _size)
                address = 0;
            mac = new MacAddress(address);
            return count == _size;
        }

        public override string ToString() => string.Join(":", GetBytes().Select(b => b.ToString("X2")));

        private IEnumerable<byte> GetBytes()
        {
            var address = RawValue;
            for (int i = 0; i < _size; i++)
            {
                int offset = (_size - 1 - i) * 8;
                byte b = (byte)(address >> offset);
                yield return b;
            }
        }

        private static ulong Convert(IEnumerable<byte> bytes, out int count)
        {
            count = 0;
            ulong address = 0;
            foreach (byte b in bytes)
            {
                address = address << 8;
                address |= b;
                count++;
            }

            return address;
        }

        public static implicit operator MacAddress(ulong mac) => new MacAddress(mac);
        public static implicit operator ulong(MacAddress mac) => mac.RawValue;
    }
}
