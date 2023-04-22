using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace JS.Abp.DataPermission.Demos
{
    public interface IDemoRepository : IRepository<Demo, Guid>
    {
        Task<Demo> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Demo>> GetListAsync(
            string filterText = null,
            string name = null,
            string displayName = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            string name = null,
            string displayName = null,
            CancellationToken cancellationToken = default);
    }
}