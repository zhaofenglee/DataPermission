using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace JS.Abp.DataPermission;

public interface IOrganizationStore
{
    /// <summary>
    /// 获取组织单元成员
    /// </summary>
    /// <param name="id">UserId</param>
    /// <returns></returns>
    Task<List<string>> GetMemberInOrganizationUnitAsync(Guid id);
    /// <summary>
    /// 获取用户组织单元领导
    /// </summary>
    /// <param name="id">UserId</param>
    /// <param name="filter">组织过滤</param>
    /// <returns></returns>
    Task<List<IdentityUserWithLevel>> GetLeadersInOrganizationUnitAsync(Guid id,string filter = null);
}