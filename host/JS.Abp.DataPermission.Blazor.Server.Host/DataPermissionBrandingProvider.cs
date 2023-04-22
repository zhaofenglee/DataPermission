using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace JS.Abp.DataPermission.Blazor.Server.Host;

[Dependency(ReplaceServices = true)]
public class DataPermissionBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "DataPermission";
}
