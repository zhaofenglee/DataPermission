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

namespace JS.Abp.DataPermission.Demos
{
    public class EfCoreDemoRepository : EfCoreRepository<DataPermissionDbContext, Demo, Guid>, IDemoRepository
    {
        protected IDataPermissionStore dataPermissionStore => LazyServiceProvider.LazyGetRequiredService<IDataPermissionStore>();
        
        public EfCoreDemoRepository(IDbContextProvider<DataPermissionDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
        public async Task<Demo> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            query = dataPermissionStore.EntityFilter(query); //add
            var item = await query.FirstOrDefaultAsync(e => e.Id == id, GetCancellationToken(cancellationToken));
            var getPermission = await dataPermissionStore.GetPermissionAsync(id.ToString(), item); //add
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
            var query = ApplyFilter((await GetQueryableAsync()), filterText, name, displayName);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DemoConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string name = null,
            string displayName = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, name, displayName);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Demo> ApplyFilter(
            IQueryable<Demo> query,
            string filterText,
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