using System;

namespace JS.Abp.DataPermission.PermissionItems;

[Serializable]
public class PermissionItemExcelDownloadTokenCacheItem
{
    public string Token { get; set; }
}