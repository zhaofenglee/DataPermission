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

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public class EfCoreObjectPermissionRepository : EfCoreRepository<DataPermissionDbContext, ObjectPermission, Guid>, IObjectPermissionRepository
    {
        public EfCoreObjectPermissionRepository(IDbContextProvider<DataPermissionDbContext> dbContextProvider)
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
            var query = ApplyFilter((await GetQueryableAsync()), filterText, objectName, description);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ObjectPermissionConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string objectName = null,
            string description = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, objectName, description);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
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