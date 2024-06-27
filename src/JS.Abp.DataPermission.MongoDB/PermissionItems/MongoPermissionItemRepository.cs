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

namespace JS.Abp.DataPermission.PermissionItems
{
    public class MongoPermissionItemRepository : MongoDbRepository<DataPermissionMongoDbContext, PermissionItem, Guid>, IPermissionItemRepository
    {
        public MongoPermissionItemRepository(IMongoDbContextProvider<DataPermissionMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<PermissionItem>> GetListAsync(
            string? filterText = null,
            string? objectName = null,
            string? description = null,
            string? targetType = null,
            Guid? roleId = null,
            bool? canRead = null,
            bool? canCreate = null,
            bool? canEdit = null,
            bool? canDelete = null,
            bool? isActive = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, objectName, description, targetType,roleId, canRead, canCreate, canEdit, canDelete,isActive);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PermissionItemConsts.GetDefaultSorting(false) : sorting);
            return await query.As<IMongoQueryable<PermissionItem>>()
                .PageBy<PermissionItem, IMongoQueryable<PermissionItem>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(
           string? filterText = null,
           string? objectName = null,
           string? description = null,
           string? targetType = null,
           Guid? roleId = null,
           bool? canRead = null,
           bool? canCreate = null,
           bool? canEdit = null,
           bool? canDelete = null,
           bool? isActive = null,
           CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, objectName, description, targetType,roleId, canRead, canCreate, canEdit, canDelete,isActive);
            return await query.As<IMongoQueryable<PermissionItem>>().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<PermissionItem> ApplyFilter(
            IQueryable<PermissionItem> query,
            string? filterText = null,
            string? objectName = null,
            string? description = null,
            string? targetType = null,
            Guid? roleId = null,
            bool? canRead = null,
            bool? canCreate = null,
            bool? canEdit = null,
            bool? canDelete = null,
            bool? isActive = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText),
                    e => e.ObjectName.Contains(filterText) || e.Description.Contains(filterText) ||
                         e.TargetType.Contains(filterText))
                .WhereIf(!string.IsNullOrWhiteSpace(objectName), e => e.ObjectName.Contains(objectName))
                .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description))
                .WhereIf(!string.IsNullOrWhiteSpace(targetType), e => e.TargetType.Contains(targetType))
                .WhereIf(roleId.HasValue, e => e.RoleId == roleId)
                .WhereIf(canRead.HasValue, e => e.CanRead == canRead)
                .WhereIf(canCreate.HasValue, e => e.CanCreate == canCreate)
                .WhereIf(canEdit.HasValue, e => e.CanEdit == canEdit)
                .WhereIf(canDelete.HasValue, e => e.CanDelete == canDelete)
                .WhereIf(isActive.HasValue, e => e.IsActive == isActive);
        }
    }
}