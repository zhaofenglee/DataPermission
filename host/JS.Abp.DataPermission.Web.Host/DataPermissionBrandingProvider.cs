using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace JS.Abp.DataPermission;

[Dependency(ReplaceServices = true)]
public class DataPermissionBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "DataPermission";
}
