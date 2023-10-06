using Shouldly;
using Volo.Abp.Authorization.Permissions;


namespace JS.Abp.DataPermission.Authorization.Tests;

public class AuthorizationPolicyProvider_Test:AuthorizationTestBase
{
    private readonly IPermissionChecker _permissionChecker;
    private readonly IDataPermissionAuthorizationPolicyProvider _dataPermissionAuthorizationPolicyProvider;
    
    public AuthorizationPolicyProvider_Test()
    {
        _permissionChecker = GetRequiredService<IPermissionChecker>();
        _dataPermissionAuthorizationPolicyProvider = GetRequiredService<IDataPermissionAuthorizationPolicyProvider>();
    }
    
    [Fact]
    public async Task IsGrantedAsync()
    {
        (await _permissionChecker.IsGrantedAsync("MyPermission5")).ShouldBe(true);
        (await _permissionChecker.IsGrantedAsync("UndefinedPermission")).ShouldBe(false);
    }
    [Fact]
    public async Task TestCheckAsync()
    {
        var demo = new DemoDto()
        {
            Name = "Test",
            DisplayName = "Test_Display"
        };
        
        await _dataPermissionAuthorizationPolicyProvider.CheckAsync(demo);
        demo.Name.ShouldBe("Test");
        demo.DisplayName.ShouldBe(null);
    }
    [Fact]
    public async Task TestCheckListAsync()
    {
        List<DemoDto> demos = new List<DemoDto>()
        {
            new DemoDto()
            {
                Name = "Test1",
                DisplayName = "Test_Display1"
            },
            new DemoDto()
            {
                Name = "Test2",
                DisplayName = "Test_Display2"
            }
        };
        
        await _dataPermissionAuthorizationPolicyProvider.CheckListAsync(demos);
        demos[0].Name.ShouldBe("Test1");
        demos[0].DisplayName.ShouldBe(null);
        demos[1].Name.ShouldBe("Test2");
        demos[1].DisplayName.ShouldBe(null);
    }
}