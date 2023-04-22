using JS.Abp.DataPermission.Localization;
using Volo.Abp.Application.Services;

namespace JS.Abp.DataPermission;

public abstract class DataPermissionAppService : ApplicationService
{
    protected DataPermissionAppService()
    {
        LocalizationResource = typeof(DataPermissionResource);
        ObjectMapperContext = typeof(DataPermissionApplicationModule);
    }
    
    public void CheckDataPermission()
    {
        
    }
}
