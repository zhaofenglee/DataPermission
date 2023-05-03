namespace JS.Abp.DataPermission;

public class DataPermissionOptions
{
    public int CacheExpirationTime { get;set; }
    
    public DataPermissionOptions()
    {
        CacheExpirationTime = 10;
    }
}