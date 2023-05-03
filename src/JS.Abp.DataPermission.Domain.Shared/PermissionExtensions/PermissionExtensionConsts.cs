namespace JS.Abp.DataPermission.PermissionExtensions
{
    public static class PermissionExtensionConsts
    {
        private const string DefaultSorting = "{0}ObjectName asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "PermissionExtension." : string.Empty);
        }

        public const int ObjectNameMaxLength = 128;
        public const int DescriptionMaxLength = 128;
    }
}