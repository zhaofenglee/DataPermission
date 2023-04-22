using JS.Abp.DataPermission.PermissionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public class PermissionExtensionManager : DomainService
    {
        private readonly IPermissionExtensionRepository _permissionExtensionRepository;

        public PermissionExtensionManager(IPermissionExtensionRepository permissionExtensionRepository)
        {
            _permissionExtensionRepository = permissionExtensionRepository;
        }

        public async Task<PermissionExtension> CreateAsync(
        string objectName, Guid roleId, PermissionType permissionType, string lambdaString, bool isActive, Guid? excludedRoleId = null)
        {
            Check.NotNullOrWhiteSpace(objectName, nameof(objectName));
            Check.Length(objectName, nameof(objectName), PermissionExtensionConsts.ObjectNameMaxLength);
            Check.NotNull(permissionType, nameof(permissionType));
            Check.NotNullOrWhiteSpace(lambdaString, nameof(lambdaString));

            var permissionExtension = new PermissionExtension(
             GuidGenerator.Create(),
             objectName, roleId, permissionType, lambdaString, isActive, excludedRoleId
             );

            return await _permissionExtensionRepository.InsertAsync(permissionExtension);
        }

        public async Task<PermissionExtension> UpdateAsync(
            Guid id,
            string objectName, Guid roleId, PermissionType permissionType, string lambdaString, bool isActive, Guid? excludedRoleId = null, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(objectName, nameof(objectName));
            Check.Length(objectName, nameof(objectName), PermissionExtensionConsts.ObjectNameMaxLength);
            Check.NotNull(permissionType, nameof(permissionType));
            Check.NotNullOrWhiteSpace(lambdaString, nameof(lambdaString));

            var permissionExtension = await _permissionExtensionRepository.GetAsync(id);

            permissionExtension.ObjectName = objectName;
            permissionExtension.RoleId = roleId;
            permissionExtension.PermissionType = permissionType;
            permissionExtension.LambdaString = lambdaString;
            permissionExtension.IsActive = isActive;
            permissionExtension.ExcludedRoleId = excludedRoleId;

            permissionExtension.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _permissionExtensionRepository.UpdateAsync(permissionExtension);
        }

    }
}