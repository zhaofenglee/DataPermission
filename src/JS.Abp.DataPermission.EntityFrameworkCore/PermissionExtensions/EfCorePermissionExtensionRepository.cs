using JS.Abp.DataPermission.PermissionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using JS.Abp.DataPermission.EntityFrameworkCore;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public class EfCorePermissionExtensionRepository : EfCoreRepository<DataPermissionDbContext, PermissionExtension, Guid>, IPermissionExtensionRepository
    {
        public EfCorePermissionExtensionRepository(IDbContextProvider<DataPermissionDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<PermissionExtension>> GetListAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, objectName, roleId, excludedRoleId, permissionType, lambdaString, isActive,description);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PermissionExtensionConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string objectName = null,
            Guid? roleId = null,
            Guid? excludedRoleId = null,
            PermissionType? permissionType = null,
            string lambdaString = null,
            bool? isActive = null,
            string description = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, objectName, roleId, excludedRoleId, permissionType, lambdaString, isActive,description);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<PermissionExtension> ApplyFilter(
            IQueryable<PermissionExtension> query,
            string filterText,
            string objectName = null,
            Guid? roleId = null,
            Guid? excludedRoleId = null,
            PermissionType? permissionType = null,
            string lambdaString = null,
            bool? isActive = null,
            string description = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.ObjectName.Contains(filterText) || e.LambdaString.Contains(filterText)|| e.Description.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(objectName), e => e.ObjectName==objectName)
                    .WhereIf(roleId.HasValue, e => e.RoleId == roleId)
                    .WhereIf(excludedRoleId.HasValue, e => e.ExcludedRoleId == excludedRoleId)
                    .WhereIf(permissionType.HasValue, e => e.PermissionType == permissionType)
                    .WhereIf(!string.IsNullOrWhiteSpace(lambdaString), e => e.LambdaString.Contains(lambdaString))
                    .WhereIf(isActive.HasValue, e => e.IsActive == isActive)
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description));
        }
    }
}