using JS.Abp.DataPermission.PermissionTypes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public interface IPermissionExtensionRepository : IRepository<PermissionExtension, Guid>
    {
        Task<List<PermissionExtension>> GetListAsync(
            string filterText = null,
            string objectName = null,
            Guid? roleId = null,
            Guid? excludedRoleId = null,
            PermissionType? permissionType = null,
            string lambdaString = null,
            bool? isActive = null,
            string description = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            string objectName = null,
            Guid? roleId = null,
            Guid? excludedRoleId = null,
            PermissionType? permissionType = null,
            string lambdaString = null,
            bool? isActive = null,
            string description = null,
            CancellationToken cancellationToken = default);
    }
}