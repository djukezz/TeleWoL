using Telegram.Bot.Types;

namespace TeleWoL;

internal sealed class UserContext
{
    public UserContext(User user)
    {
        UserId = user.Id;
        UserName = user.Username ?? string.Empty;
        FirstName = user.FirstName ?? string.Empty;
        LastName = user.LastName ?? string.Empty;
    }

    public string UserName { get; }
    public string FirstName { get; }
    public string LastName { get; } 
    public long UserId { get; }
}
