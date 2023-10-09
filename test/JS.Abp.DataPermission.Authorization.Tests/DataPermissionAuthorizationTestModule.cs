using JS.Abp.DataPermission.Authorization.Tests.TestServices;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Autofac;
using Volo.Abp.DynamicProxy;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Modularity;

namespace JS.Abp.DataPermission.Authorization.Tests;

[DependsOn(typeof(AbpAutofacModule))]
[DependsOn(typeof(AbpExceptionHandlingModule))]
[DependsOn(typeof(DataPermissionAuthorizationModule))]
public class DataPermissionAuthorizationTestModule: AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(onServiceRegistredContext =>
        {
            if (typeof(IMyAuthorizedService1).IsAssignableFrom(onServiceRegistredContext.ImplementationType) &&
                !DynamicProxyIgnoreTypes.Contains(onServiceRegistredContext.ImplementationType))
            {
                onServiceRegistredContext.Interceptors.TryAdd<AuthorizationInterceptor>();
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpPermissionOptions>(options =>
        {
            options.ValueProviders.Add<TestPermissionValueProvider1>();
            options.ValueProviders.Add<TestPermissionValueProvider2>();
        });
    }
}