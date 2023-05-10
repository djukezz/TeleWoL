namespace StateTest;

internal sealed class UserSettings
{
    public string UserName { get; set; } = string.Empty;
    public long UserId { get; set; }

    public List<Target> Targets { get; } = new List<Target>()
    {
        new Target{ Name = "Home", Mac = "AA:BB:CC:DD:EE:FF" },
        new Target{ Name = "Work", Mac = "00:11:22:33:44:55" },
    };
}
