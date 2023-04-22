using JS.Abp.DataPermission.PermissionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using JS.Abp.DataPermission.MongoDB;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public class MongoPermissionExtensionRepository : MongoDbRepository<DataPermissionMongoDbContext, PermissionExtension, Guid>, IPermissionExtensionRepository
    {
        public MongoPermissionExtensionRepository(IMongoDbContextProvider<DataPermissionMongoDbContext> dbContextProvider)
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
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, objectName, roleId, excludedRoleId, permissionType, lambdaString, isActive);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PermissionExtensionConsts.GetDefaultSorting(false) : sorting);
            return await query.As<IMongoQueryable<PermissionExtension>>()
                .PageBy<PermissionExtension, IMongoQueryable<PermissionExtension>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(
           string filterText = null,
           string objectName = null,
           Guid? roleId = null,
           Guid? excludedRoleId = null,
           PermissionType? permissionType = null,
           string lambdaString = null,
           bool? isActive = null,
           CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, objectName, roleId, excludedRoleId, permissionType, lambdaString, isActive);
            return await query.As<IMongoQueryable<PermissionExtension>>().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<PermissionExtension> ApplyFilter(
            IQueryable<PermissionExtension> query,
            string filterText,
            string objectName = null,
            Guid? roleId = null,
            Guid? excludedRoleId = null,
            PermissionType? permissionType = null,
            string lambdaString = null,
            bool? isActive = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.ObjectName.Contains(filterText) || e.LambdaString.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(objectName), e => e.ObjectName.Contains(objectName))
                    .WhereIf(roleId.HasValue, e => e.RoleId == roleId)
                    .WhereIf(excludedRoleId.HasValue, e => e.ExcludedRoleId == excludedRoleId)
                    .WhereIf(permissionType.HasValue, e => e.PermissionType == permissionType)
                    .WhereIf(!string.IsNullOrWhiteSpace(lambdaString), e => e.LambdaString.Contains(lambdaString))
                    .WhereIf(isActive.HasValue, e => e.IsActive == isActive);
        }
    }
}