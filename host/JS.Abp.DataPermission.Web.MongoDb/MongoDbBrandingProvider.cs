using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace JS.Abp.DataPermission.Web.MongoDb;

[Dependency(ReplaceServices = true)]
public class MongoDbBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "MongoDb";
}
