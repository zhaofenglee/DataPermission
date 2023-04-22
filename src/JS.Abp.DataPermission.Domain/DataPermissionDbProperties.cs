using Volo.Abp.Data;

namespace JS.Abp.DataPermission;

public static class DataPermissionDbProperties
{
    public static string DbTablePrefix { get; set; } =  AbpCommonDbProperties.DbTablePrefix;

    public static string? DbSchema { get; set; } =  AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "DataPermission";
}
