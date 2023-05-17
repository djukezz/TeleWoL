using TeleWoL.Common;

namespace TeleWoL.Settings;

internal sealed class UserSettings : IWriteable, IReadable
{
    private readonly object _lock = new object();

    public long UserId { get; set; }
    public UserPermission Permission { get; set; }
    public string UserName { get; set; } = string.Empty;

    private readonly List<Target> _targets = new List<Target>()
    {
        new Target{ Name = "Home", Mac = 0xAABBCCDDEEFF },
        new Target{ Name = "Work", Mac = 0x001122334455 },
    };

    public bool AddTarget(Target target)
    {
        if (string.IsNullOrWhiteSpace(target.Name))
            return false;

        lock (_lock)
        {
            if (_targets.Contains(target))
                return false;

            _targets.Add(target);
            return true;
        }
    }

    public bool DeleteTarget(Target target)
    {
        lock (_lock)
        {
            return _targets.Remove(target);
        }
    }

    public IEnumerable<Target> GetTargets()
    {
        lock (_lock)
        {
            foreach (var target in _targets)
                yield return target;
        }
    }

    public void Write(BinaryWriter bw)
    {
        lock (_lock)
        {
            bw.Write(UserId);
            bw.Write(UserName);
            bw.Write(_targets);
        }
    }

    public void Read(BinaryReader br)
    {
        lock (_lock)
        {
            UserId = br.ReadInt64();
            UserName = br.ReadString();
            _targets.Clear();
            br.Read(_targets);
        }
    }
}

internal enum UserPermission : byte
{
    User,
    Admin,
}
