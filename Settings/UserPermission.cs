namespace TeleWoL.Settings;

internal enum UserPermission : byte
{
    None,
    User,
    Admin,
}

internal static class UserPermissionEx
{
    public static UserPermission Max(UserPermission p1, UserPermission p2)
    {
        if (p1 == UserPermission.Admin || p2 == UserPermission.Admin)
            return UserPermission.Admin;
        if (p1 == UserPermission.User || p2 == UserPermission.User)
            return UserPermission.User;
        return UserPermission.None;
    }
}
