using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using JS.Abp.DataPermission.ObjectPermissions;

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public class ObjectPermissionsDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IObjectPermissionRepository _objectPermissionRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ObjectPermissionsDataSeedContributor(IObjectPermissionRepository objectPermissionRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _objectPermissionRepository = objectPermissionRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _objectPermissionRepository.InsertAsync(new ObjectPermission
            (
                id: Guid.Parse("0e96e30d-462c-4b6e-a92c-8a538dafdab2"),
                objectName: "53a3ee1a28ff443984bf9a2e95a9f05c7c1c8a77e1ff42a1864d48afac7f3acb716ffd81c76148ecb03a3223bf53dc8be986f48096c94fc18a07b32e64f96398",
                description: "8d195358b663424e819670724d04021100feb2a79b424cf9a"
            ));

            await _objectPermissionRepository.InsertAsync(new ObjectPermission
            (
                id: Guid.Parse("43e88ac8-c5d0-4085-a2b3-8528cf938e74"),
                objectName: "3f1ac3871b2b4e9d9a14a21217b51f65577673c5aa914620bb96f3a580cc2b24e3d6d23ae3144a309c287623793e0e117a3ee07d24f34f9bbde95403f9a90f48",
                description: "9099bdfdc7084050b42d611d714007c8e484fd6d502446"
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}