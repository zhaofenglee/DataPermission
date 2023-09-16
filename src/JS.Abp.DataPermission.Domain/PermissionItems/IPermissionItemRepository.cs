using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace JS.Abp.DataPermission.PermissionItems
{
    public interface IPermissionItemRepository : IRepository<PermissionItem, Guid>
    {
        Task<List<PermissionItem>> GetListAsync(
            string filterText = null,
            string objectName = null,
            string description = null,
            string targetType = null,
            Guid? roleId = null,
            bool? canRead = null,
            bool? canCreate = null,
            bool? canEdit = null,
            bool? canDelete = null,
            bool? isActive = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            string objectName = null,
            string description = null,
            string targetType = null,
            Guid? roleId = null,
            bool? canRead = null,
            bool? canCreate = null,
            bool? canEdit = null,
            bool? canDelete = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);
    }
}