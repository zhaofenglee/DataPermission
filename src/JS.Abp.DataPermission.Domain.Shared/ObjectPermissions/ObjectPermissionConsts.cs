namespace JS.Abp.DataPermission.ObjectPermissions
{
    public static class ObjectPermissionConsts
    {
        private const string DefaultSorting = "{0}ObjectName asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "ObjectPermission." : string.Empty);
        }

        public const int ObjectNameMaxLength = 128;
    }
}