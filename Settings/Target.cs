using Newtonsoft.Json;

namespace TeleWoL.Settings;

[JsonObject(MemberSerialization.OptIn)]
internal sealed class Target : IEquatable<Target>
{
    [JsonProperty]
    public string Name { get; set; } = string.Empty;
    [JsonProperty]
    public MacAddress Mac { get; set; }

    public bool Equals(Target? other) => Name == other?.Name;
    public override bool Equals(object? obj) => Equals(obj as Target);
    public override int GetHashCode() => Name.GetHashCode();
    public override string ToString() => $"'{Name}' {Mac}";
}
