using Volo.Abp.Bundling;

namespace JS.Abp.DataPermission.Blazor.Host;

public class DataPermissionBlazorHostBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {

    }

    public void AddStyles(BundleContext context)
    {
        context.Add("main.css", true);
    }
}
