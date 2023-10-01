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

    public bool Update(UserContext userContext)
    {
        lock (_lock)
        {
            bool isChanged = Update(() => UserName, v => UserName = v, userContext.UserName) |
                Update(() => FirstName, v => FirstName = v, userContext.FirstName) |
                Update(() => LastName, v => LastName = v, userContext.LastName);
            return isChanged;
        }
    }

    private bool Update<T>(Func<T> get, Action<T> set, T value)
    {
        var old = get();
        if(EqualityComparer<T>.Default.Equals(get(), value))
            return false;
        set(value);
        return true;
    }

    public override string ToString()
    {
        var parts = new[] { UserName, FirstName, LastName }
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .DefaultIfEmpty(UserId.ToString());
        if (Permission == UserPermission.Admin)
            parts = parts.Prepend("^");

        return string.Join(" ", parts);
    }
}
