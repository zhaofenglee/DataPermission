using System;
using System.Threading.Tasks;
using JS.Abp.DataPermission;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace JS.Abp.DataPermission.Identity;

public class IdentityDataSeedContributor : IDataSeedContributor, ISingletonDependency
{
    private bool IsSeeded = false;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IIdentityRoleRepository _roleRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
        
    public IdentityDataSeedContributor(IIdentityUserRepository userRepository, IIdentityRoleRepository roleRepository, IUnitOfWorkManager unitOfWorkManager)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWorkManager = unitOfWorkManager;
    }


    public async Task SeedAsync(DataSeedContext context)
    {
        if (IsSeeded)
        {
            return;
        }
        await _roleRepository.InsertAsync(new IdentityRole
            (TestData.RoleId1,
                "Admin"));
        
        IdentityUser user1 =new IdentityUser
        (TestData.UserId1,
            "test",
            "test@test.com");
        user1.AddRole(TestData.RoleId1);
        await _userRepository.InsertAsync(user1);
        
        IdentityUser user2 =new IdentityUser
        (TestData.UserId2,
            "test2",
            "test2@test.com");
        await _userRepository.InsertAsync(user2);
        
        await _unitOfWorkManager.Current.SaveChangesAsync();

        IsSeeded = true;
        
    }
}
