namespace TeleWoL.Common;

internal static class ReadableWriteableEx
{
    public static void Write<T>(this BinaryWriter bw, IReadOnlyCollection<T> items)
        where T : IWriteable
    {
        bw.Write(items.Count);
        foreach (var item in items)
            item.Write(bw);
    }

    public static IReadOnlyList<T> Read<T>(this BinaryReader br)
        where T : IReadable, new()
    {
        var res = new List<T>();
        Read(br, res);
        return res;
    }

    public static void Read<T>(this BinaryReader br, IList<T> target)
    where T : IReadable, new()
    {
        int count = br.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            T item = new T();
            item.Read(br);
            target.Add(item);
        }
    }
}