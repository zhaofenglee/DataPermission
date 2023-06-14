using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace JS.Abp.DataPermission;

public class OrganizationStore: IOrganizationStore, ITransientDependency
{
    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly IOrganizationUnitRepository _organizationUnitRepository;
    
    public OrganizationStore(IIdentityUserRepository identityUserRepository, IOrganizationUnitRepository organizationUnitRepository)
    {
        _identityUserRepository = identityUserRepository;
        _organizationUnitRepository = organizationUnitRepository;
    }
    
    public virtual async Task<List<string>> GetMenberInOrganizationUnitAsync(Guid id)
    {
        List<string> result = new List<string>();
        result.Add(id.ToString());
        var oulist = await _identityUserRepository.GetOrganizationUnitsAsync(id,true);
        if (oulist.Any())
        {
            foreach (var ou in oulist)
            {
                var menbers =await _organizationUnitRepository.GetUnaddedUsersAsync(ou);
                if (menbers.Any())
                {
                    foreach (var menber in menbers)
                    {
                        result.Add(menber.Id.ToString());
        
                    }
                                    
                }
            }
        }
        //去重后返回
        result = result.Distinct().ToList();

        return result;
    }
}