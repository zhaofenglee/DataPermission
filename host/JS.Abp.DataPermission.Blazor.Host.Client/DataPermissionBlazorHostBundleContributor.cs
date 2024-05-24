using Volo.Abp.Bundling;

namespace JS.Abp.DataPermission.Blazor.Host.Client;

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
