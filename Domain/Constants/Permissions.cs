namespace Domain.Constants;

public static class Permissions
{
    public static List<string> GeneratePermissionsForModule(string module)
    {
        return
        [
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete"
        ];
    }
    public static class Meetings
    {
        public const string View = "Permissions.Meetings.View";
        public const string Create = "Permissions.Meetings.Create";
        public const string Edit = "Permissions.Meetings.Edit";
        public const string Delete = "Permissions.Meetings.Delete";
    }

    public static class Notifications
    {
        public const string View = "Permissions.Notifications.View";
        public const string Create = "Permissions.Notifications.Create";
        public const string Edit = "Permissions.Notifications.Edit";
        public const string Delete = "Permissions.Notifications.Delete";
    }

    public static class Roles
    {
        public const string View = "Permissions.Roles.View";
        public const string Create = "Permissions.Roles.Create";
        public const string Edit = "Permissions.Roles.Edit";
        public const string Delete = "Permissions.Roles.Delete";
    }


    public static class Users
    {
        public const string View = "Permissions.Users.View";
        public const string Create = "Permissions.Users.Create";
        public const string Edit = "Permissions.Users.Edit";
        public const string Delete = "Permissions.Users.Delete";
    }

    public static class UserRoles
    {
        public const string View = "Permissions.UserRoles.View";
        public const string Create = "Permissions.UserRoles.Create";
        public const string Edit = "Permissions.UserRoles.Edit";
        public const string Delete = "Permissions.UserRoles.Delete";
    }
}