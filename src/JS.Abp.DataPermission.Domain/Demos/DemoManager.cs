using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JS.Abp.DataPermission.PermissionTypes;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace JS.Abp.DataPermission.Demos
{
    public class DemoManager : DomainService
    {
        private readonly IDemoRepository _demoRepository;
        protected IDataPermissionStore dataPermissionStore => LazyServiceProvider.LazyGetRequiredService<IDataPermissionStore>();//add
        public DemoManager(IDemoRepository demoRepository)
        {
            _demoRepository = demoRepository;
        }

        public async Task<Demo> CreateAsync(
        string name, string displayName)
        {
            Check.Length(name, nameof(name), DemoConsts.NameMaxLength);
            Check.Length(displayName, nameof(displayName), DemoConsts.DisplayNameMaxLength);

            var demo = new Demo(
             GuidGenerator.Create(),
             name, displayName
             );
            //add permission check
            if (!dataPermissionStore.CheckPermission(demo, PermissionType.Create))
            {
                throw new UserFriendlyException(
                    "The current user does not have permission to create data!"
                );
            }
            return await _demoRepository.InsertAsync(demo);
        }

        public async Task<Demo> UpdateAsync(
            Guid id,
            string name, string displayName, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.Length(name, nameof(name), DemoConsts.NameMaxLength);
            Check.Length(displayName, nameof(displayName), DemoConsts.DisplayNameMaxLength);

            var demo = await _demoRepository.GetAsync(id);

            demo.Name = name;
            demo.DisplayName = displayName;

            demo.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _demoRepository.UpdateAsync(demo);
        }

    }
}