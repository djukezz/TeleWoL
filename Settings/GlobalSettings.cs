using Newtonsoft.Json;

namespace TeleWoL.Settings;

[JsonObject(MemberSerialization.OptIn)]
internal sealed class GlobalSettings
{
    private readonly object _lock = new object();
    [JsonProperty]
    private List<UserSettings> Users { get; set; } = new List<UserSettings>();

    [JsonProperty]
    public string AdminPassword { get; set; } = "123456";
    [JsonProperty]
    public string UserPassword { get; set; } = "1234";
    [JsonProperty]
    public string Token { get; set; } = "6050754775:AAFQUT_kLk8uzvvDDbaIk6hnDWJrkvTY-Mw";
    [JsonProperty]
    public string Subnet { get; set; } = "127.0.0.1";
    [JsonProperty]
    public string SubnetMask { get; set; } = "255.255.255.0";

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
        if (!string.IsNullOrEmpty(AdminPassword) && password == AdminPassword)
            return UserPermission.Admin;
        if (!string.IsNullOrEmpty(UserPassword) && password == UserPassword)
            return UserPermission.User;
        return UserPermission.None;
    }
}
