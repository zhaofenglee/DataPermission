using Volo.Abp.Reflection;

namespace JS.Abp.DataPermission.Permissions;

public class DataPermissionPermissions
{
    public const string GroupName = "DataPermission";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(DataPermissionPermissions));
    }

    public static class PermissionExtensions
    {
        public const string Default = GroupName + ".PermissionExtensions";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Demos
    {
        public const string Default = GroupName + ".Demos";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}