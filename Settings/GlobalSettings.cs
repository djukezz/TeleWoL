using Newtonsoft.Json;

namespace TeleWoL.Settings;

[JsonObject(MemberSerialization.OptIn)]
internal sealed class GlobalSettings
{
    private readonly object _lock = new object();
    [JsonProperty]
    private List<UserSettings> Users { get; set; } = new List<UserSettings>();

    [JsonProperty]
    public string AdminPassword { get; set; } = string.Empty;
    [JsonProperty]
    public string UserPassword { get; set; } = string.Empty;
    [JsonProperty]
    public string Token { get; set; } = string.Empty;
    [JsonProperty]
    public string Subnet { get; set; } = string.Empty;
    [JsonProperty]
    public string SubnetMask { get; set; } = string.Empty;

    public UserSettings? Get(long userId)
    {
        lock (_lock)
        {
            return Users.FirstOrDefault(x => x.UserId == userId);
        }
    }

    public bool Delete(long userId)
    {
        lock (_lock)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].UserId == userId)
                {
                    Users.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }

    public UserSettings GetOrAdd(long userId)
    {
        lock (_lock)
        {
            var user = Users.FirstOrDefault(x => x.UserId == userId);
            if (user == null)
            {
                user = new UserSettings { UserId = userId };
                Users.Add(user);
            }
            return user;
        }
    }

    public IEnumerable<UserSettings> GetAll()
    {
        lock (_lock)
        {
            foreach (var user in Users)
                yield return user;
        }
    }

    public UserPermission GetPermission(string password)
    {
        if (password == Token)
            return UserPermission.Admin;
        if (!string.IsNullOrEmpty(AdminPassword) && password == AdminPassword)
            return UserPermission.Admin;
        if (!string.IsNullOrEmpty(UserPassword) && password == UserPassword)
            return UserPermission.User;
        return UserPermission.None;
    }
}
