﻿using TeleWoL.Common;

namespace TeleWoL.Settings;

internal sealed class Target : IEquatable<Target>, IWriteable, IReadable
{
    public string Name { get; set; } = string.Empty;
    public MacAddress Mac { get; set; }

    public void Read(BinaryReader br)
    {
        Name = br.ReadString();
        Mac = br.ReadUInt64();
    }

    public void Write(BinaryWriter bw)
    {
        bw.Write(Name);
        bw.Write(Mac);
    }

    public bool Equals(Target? other) => Name == other?.Name;
    public override bool Equals(object? obj) => Equals(obj as Target);
    public override int GetHashCode() => Name.GetHashCode();
    public override string ToString() => $"'{Name}' {Mac}";
}
