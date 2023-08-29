using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JS.Abp.DataPermission;

public interface IOrganizationStore
{
    Task<List<string>> GetMemberInOrganizationUnitAsync(Guid id);
}