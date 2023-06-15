using Newtonsoft.Json;

namespace TeleWoL.Settings;

[JsonObject(MemberSerialization.OptIn)]
internal sealed class UserSettings
{
    private readonly object _lock = new object();

    [JsonProperty]
    public long UserId { get; set; }
    [JsonProperty]
    public UserPermission Permission { get; set; }
    [JsonProperty]
    public string UserName { get; set; } = string.Empty;
    [JsonProperty]
    public string FirstName { get; set; } = string.Empty;
    [JsonProperty]
    public string LastName { get; set; } = string.Empty;

    [JsonProperty]
    private List<Target> Targets { get; set; } = new List<Target>();

    public bool AddTarget(Target target)
    {
        if (string.IsNullOrWhiteSpace(target.Name))
            return false;

        lock (_lock)
        {
            if (Targets.Contains(target))
                return false;

            Targets.Add(target);
            return true;
        }
    }

    public bool DeleteTarget(Target target)
    {
        lock (_lock)
        {
            return Targets.Remove(target);
        }
    }

    public IEnumerable<Target> GetTargets()
    {
        lock (_lock)
        {
            foreach (var target in Targets)
                yield return target;
        }
    }

    public void Update(UserContext userContext)
    {
        lock (_lock)
        {
            UserName = userContext.UserName;
            FirstName = userContext.FirstName;
            LastName = userContext.LastName;
        }
    }
}
