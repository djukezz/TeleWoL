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

    public UserSettings? Get(long userId)
    {
        lock (_lock)
        {
            return Users.FirstOrDefault(x => x.UserId == userId);
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

    public void Read(BinaryReader br)
    {
        lock (_lock)
        {
            AdminPassword = br.ReadString();
            _users.Clear();
            br.Read(_users);
        }
    }

    public void Write(BinaryWriter bw)
    {
        lock (_lock)
        {
            bw.Write(AdminPassword ?? string.Empty);
            bw.Write(_users);
        }
    }
}
