using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public interface IObjectPermissionRepository : IRepository<ObjectPermission, Guid>
    {
        Task<List<ObjectPermission>> GetListAsync(
            string filterText = null,
            string objectName = null,
            string description = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            string objectName = null,
            string description = null,
            CancellationToken cancellationToken = default);
    }
}