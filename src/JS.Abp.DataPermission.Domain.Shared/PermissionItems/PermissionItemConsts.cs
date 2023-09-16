namespace JS.Abp.DataPermission.PermissionItems
{
    public static class PermissionItemConsts
    {
        private const string DefaultSorting = "{0}ObjectName asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "PermissionItem." : string.Empty);
        }

        public const int ObjectNameMaxLength = 128;
        public const int TargetTypeMaxLength = 128;
    }
}