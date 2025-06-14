using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace JS.Abp.DataPermission.Blazor.Host.Client;

/* Add your global styles/scripts here.
 * See https://docs.abp.io/en/abp/latest/UI/Blazor/Global-Scripts-Styles to learn how to use it
 */
public class DataPermissionBlazorHostBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add(new BundleFile("main.css", true));
    }
}
