using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using JS.Abp.DataPermission.PermissionItems;

namespace JS.Abp.DataPermission.PermissionItems
{
    public class PermissionItemsDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IPermissionItemRepository _permissionItemRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PermissionItemsDataSeedContributor(IPermissionItemRepository permissionItemRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _permissionItemRepository = permissionItemRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _permissionItemRepository.InsertAsync(new PermissionItem
            (
                id: Guid.Parse("b3f400c1-6974-4c36-9275-05d472458284"),
                objectName: "cf314826ecd44317b980aa74c65f3d6b9444b3af869a4456bfbf2970bdf7eba96d356c758b354e4182719a8fc4ae4570e5c583cf513d4cd0bedb577e6e5b1bfa",
                description: "906527147df94bd1b3e7e8bd825f4c188b192898b2fe42a58ca06ade19fe54601e0e",
                targetType: "2db7a10af04e40b4a0d62cd2af9aac61900c5d947c11487ab21d1491ae74aef2dfd13be23df344df88e67023dadd6249632bc4eddf6a4c40b52336da5f24670d",
                roleId: Guid.Parse("00000000-0000-0000-0000-000000000000"),
                canRead: true,
                canCreate: true,
                canEdit: true,
                canDelete: true,
                isActive: true
            ));

            await _permissionItemRepository.InsertAsync(new PermissionItem
            (
                id: Guid.Parse("721fb0ec-9705-44c3-ac98-58eb377a7597"),
                objectName: "cb67d9d3da8a49bbb0e159d1d201fab471ea261b78c244dabbd1ca0d7ce0f2418920284a37c64f69bf1a57f07cfdba079c7d87442ca64d13aec3e0a3a5542322",
                description: "56c9b0e9749949e780a17b681a934",
                targetType: "072408bac31a488897e962c660178c1540b3c3062bbb41fca483107b2de1993caec2483984b744b2b2d58322b8a4ba4f6e88549322124cc3a6a1fbc090195547",
                roleId: Guid.Parse("00000000-0000-0000-0000-000000000000"),
                canRead: true,
                canCreate: true,
                canEdit: true,
                canDelete: true,
                isActive: true
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}