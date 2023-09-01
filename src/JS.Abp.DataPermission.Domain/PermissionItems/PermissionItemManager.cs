using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace JS.Abp.DataPermission.PermissionItems
{
    public class PermissionItemManager : DomainService
    {
        private readonly IPermissionItemRepository _permissionItemRepository;

        public PermissionItemManager(IPermissionItemRepository permissionItemRepository)
        {
            _permissionItemRepository = permissionItemRepository;
        }

        public async Task<PermissionItem> CreateAsync(
        string objectName, string description, string targetType,Guid roleId, bool canRead, bool canCreate, bool canEdit, bool canDelete, bool isActive)
        {
            Check.NotNull(objectName, nameof(objectName));
            Check.Length(objectName, nameof(objectName), PermissionItemConsts.ObjectNameMaxLength, 0);
            Check.NotNull(targetType, nameof(targetType));
            Check.Length(targetType, nameof(targetType), PermissionItemConsts.TargetTypeMaxLength, 0);

            var permissionItem = new PermissionItem(
             GuidGenerator.Create(),
             objectName, description, targetType,roleId, canRead, canCreate, canEdit, canDelete,isActive
             );

            return await _permissionItemRepository.InsertAsync(permissionItem);
        }

        public async Task<PermissionItem> UpdateAsync(
            Guid id,
            string objectName, string description, string targetType,Guid roleId, bool canRead, bool canCreate, bool canEdit, bool canDelete, bool isActive, [CanBeNull] string concurrencyStamp = null
        )
        {
          
            Check.NotNull(objectName, nameof(objectName));
            Check.Length(objectName, nameof(objectName), PermissionItemConsts.ObjectNameMaxLength, 0);
            Check.NotNull(targetType, nameof(targetType));
            Check.Length(targetType, nameof(targetType), PermissionItemConsts.TargetTypeMaxLength, 0);

            var permissionItem = await _permissionItemRepository.GetAsync(id);

            permissionItem.Description = description;
            permissionItem.TargetType = targetType;
            permissionItem.RoleId = roleId;
            permissionItem.CanRead = canRead;
            permissionItem.CanCreate = canCreate;
            permissionItem.CanEdit = canEdit;
            permissionItem.CanDelete = canDelete;
            permissionItem.IsActive = isActive;

            permissionItem.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _permissionItemRepository.UpdateAsync(permissionItem);
        }

    }
}