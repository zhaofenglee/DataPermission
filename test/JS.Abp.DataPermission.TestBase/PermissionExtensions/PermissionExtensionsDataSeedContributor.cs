using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using JS.Abp.DataPermission.PermissionExtensions;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public class PermissionExtensionsDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IPermissionExtensionRepository _permissionExtensionRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PermissionExtensionsDataSeedContributor(IPermissionExtensionRepository permissionExtensionRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _permissionExtensionRepository = permissionExtensionRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _permissionExtensionRepository.InsertAsync(new PermissionExtension
            (
                id: Guid.Parse("064a532a-ae12-44d4-8291-36f3cac0f284"),
                objectName: "fd5072d8b9664eccb54431f19420e06cf066f43e44234830b3dc10df579d910f382aa3cb0dbe4097aa96d7eb58334064d3d7987e14a847969a0dc8e6b345cfc2",
                roleId: Guid.Parse("3e5ca2f9-fb65-4819-b4df-e62cae198ae8"),
                excludedRoleId: Guid.Parse("e7a0e05f-2cca-4d95-a51d-32c433b21a33"),
                permissionType: default,
                lambdaString: "52405753f73641bdb43a5261",
                isActive: true,
                description:null
            ));

            await _permissionExtensionRepository.InsertAsync(new PermissionExtension
            (
                id: Guid.Parse("1f6c5bc9-9e00-4a21-80b4-d4e7b217f088"),
                objectName: "f4f090cd6e794692928a64e8c7b6cc7a94d0bac36fd64e6a95813b8c41c290a40cb2b56f8ab64e7b8f46e3997057a505607b4e1e4fbc495b885c68fb03ac558d",
                roleId: Guid.Parse("74202aef-0b0d-4ae1-a9cb-85e4cbf8c06e"),
                excludedRoleId: Guid.Parse("2832d88e-33e9-47ed-9b54-8b8804ca6e12"),
                permissionType: default,
                lambdaString: "08676f6",
                isActive: true,
                description:null
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}