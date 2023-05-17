using TeleWoL.Common;

namespace TeleWoL.Settings;

internal sealed class GlobalSettings : IReadable, IWriteable
{
    private readonly object _lock = new object();
    private readonly List<UserSettings> _users = new List<UserSettings>();

    public string AdminPassword { get; set; } = "123456";
    public string UserPassword { get; set; } = "1234";

    public UserSettings? Get(long userId)
    {
        lock (_lock)
        {
            return _users.FirstOrDefault(x => x.UserId == userId);
        }
    }

    public UserSettings GetOrAdd(long userId)
    {
        lock (_lock)
        {
            var user = _users.FirstOrDefault(x => x.UserId == userId);
            if (user == null)
            {
                user = new UserSettings { UserId = userId };
                _users.Add(user);
            }
            return user;
        }
    }

    public IEnumerable<UserSettings> GetAll()
    {
        lock (_lock)
        {
            foreach (var user in _users)
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
