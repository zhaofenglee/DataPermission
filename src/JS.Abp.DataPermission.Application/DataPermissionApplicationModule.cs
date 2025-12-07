using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.Mapperly;

namespace JS.Abp.DataPermission;

[DependsOn(
    typeof(DataPermissionDomainModule),
    typeof(DataPermissionApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpMapperlyModule)
    )]
public class DataPermissionApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<DataPermissionApplicationModule>();
        context.Services.AddEntityCache<IdentityRole, Guid>();

    }
}
