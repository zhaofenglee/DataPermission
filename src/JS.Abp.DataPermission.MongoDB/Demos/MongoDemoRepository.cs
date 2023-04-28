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

namespace JS.Abp.DataPermission.Demos
{
    public class MongoDemoRepository : MongoDbRepository<DataPermissionMongoDbContext, Demo, Guid>, IDemoRepository
    {
        protected IDataPermissionStore dataPermissionStore => LazyServiceProvider.LazyGetRequiredService<IDataPermissionStore>();
        public MongoDemoRepository(IMongoDbContextProvider<DataPermissionMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Demo> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter(await GetMongoQueryableAsync(cancellationToken));
            var item = query.FirstOrDefault(e => e.Id == id);
            await dataPermissionStore.GetPermissionAsync(item); //add
            return item;
        }

        public async Task<List<Demo>> GetListAsync(
            string filterText = null,
            string name = null,
            string displayName = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, name, displayName);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DemoConsts.GetDefaultSorting(false) : sorting);
            return await query.As<IMongoQueryable<Demo>>()
                .PageBy<Demo, IMongoQueryable<Demo>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(
           string filterText = null,
           string name = null,
           string displayName = null,
           CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, name, displayName);
            return await query.As<IMongoQueryable<Demo>>().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Demo> ApplyFilter(
            IQueryable<Demo> query,
            string filterText = null,
            string name = null,
            string displayName = null)
        {
            query = dataPermissionStore.EntityFilter(query);//add
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name.Contains(filterText) || e.DisplayName.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
                    .WhereIf(!string.IsNullOrWhiteSpace(displayName), e => e.DisplayName.Contains(displayName));
        }
    }
}