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

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public class MongoObjectPermissionRepository : MongoDbRepository<DataPermissionMongoDbContext, ObjectPermission, Guid>, IObjectPermissionRepository
    {
        public MongoObjectPermissionRepository(IMongoDbContextProvider<DataPermissionMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ObjectPermission>> GetListAsync(
            string filterText = null,
            string objectName = null,
            string description = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, objectName, description);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ObjectPermissionConsts.GetDefaultSorting(false) : sorting);
            return await query.As<IMongoQueryable<ObjectPermission>>()
                .PageBy<ObjectPermission, IMongoQueryable<ObjectPermission>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(
           string filterText = null,
           string objectName = null,
           string description = null,
           CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, objectName, description);
            return await query.As<IMongoQueryable<ObjectPermission>>().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<ObjectPermission> ApplyFilter(
            IQueryable<ObjectPermission> query,
            string filterText,
            string objectName = null,
            string description = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.ObjectName.Contains(filterText) || e.Description.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(objectName), e => e.ObjectName.Contains(objectName))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description));
        }
    }
}