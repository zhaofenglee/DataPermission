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
    
    public virtual async Task<List<string>> GetMemberInOrganizationUnitAsync(Guid id)
    {
        List<string> result = new List<string>();
        result.Add(id.ToString());
        var ouList = await _identityUserRepository.GetOrganizationUnitsAsync(id,false);
        if (ouList.Any())
        {
            foreach (var ou in ouList)
            {
                var menbers =await _organizationUnitRepository.GetMembersAsync(ou);
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

    public virtual async Task<List<IdentityUserWithLevel>> GetLeadersInOrganizationUnitAsync(Guid id,string filter = null)
    {
        var ouList = await _identityUserRepository.GetOrganizationUnitsAsync(id,false);
        if (!filter.IsNullOrWhiteSpace())
        {
            ouList = ouList.Where(x => x.DisplayName.Contains(filter)).ToList();
        }
        List<IdentityUserWithLevel> result = new List<IdentityUserWithLevel>();
        if (ouList.Any())
        {
            foreach (var ou in ouList)
            {
                await AddLeadersInOrganizationUnitAsync(ou, result,0);
            }
        }
        return result;
    }
    
    private async Task AddLeadersInOrganizationUnitAsync(OrganizationUnit ou, List<IdentityUserWithLevel> result, int level)
    {
        if (ou.ParentId.HasValue)
        {
            var leaderOrganizationUnit = await _organizationUnitRepository.GetAsync(ou.ParentId.Value);
            var members = await _organizationUnitRepository.GetMembersAsync(leaderOrganizationUnit);
            foreach (var member in members)
            {
                result.Add(new IdentityUserWithLevel { IdentityUser = member, Level = level });
            }
            await AddLeadersInOrganizationUnitAsync(leaderOrganizationUnit, result, level + 1);
        }
    }
}