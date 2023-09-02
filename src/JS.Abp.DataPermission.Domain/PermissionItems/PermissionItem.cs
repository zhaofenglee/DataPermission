using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace JS.Abp.DataPermission.PermissionItems
{
    public class PermissionItem : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string ObjectName { get; set; }

        [CanBeNull]
        public virtual string? Description { get; set; }

        [NotNull]
        public virtual string TargetType { get; set; }
        public virtual Guid RoleId { get; set; }
        public virtual bool CanRead { get; set; }

        public virtual bool CanCreate { get; set; }

        public virtual bool CanEdit { get; set; }

        public virtual bool CanDelete { get; set; }
        public virtual bool IsActive { get; set; }

        public PermissionItem()
        {

        }

        public PermissionItem(Guid id, string objectName, string description, string targetType, Guid roleId,  bool canRead, bool canCreate, bool canEdit, bool canDelete,bool isActive)
        {

            Id = id;
            Check.NotNull(objectName, nameof(objectName));
            Check.Length(objectName, nameof(objectName), PermissionItemConsts.ObjectNameMaxLength, 0);
            Check.NotNull(targetType, nameof(targetType));
            Check.Length(targetType, nameof(targetType), PermissionItemConsts.TargetTypeMaxLength, 0);
            ObjectName = objectName;
            RoleId = roleId;
            Description = description;
            TargetType = targetType;
            CanRead = canRead;
            CanCreate = canCreate;
            CanEdit = canEdit;
            CanDelete = canDelete;
            IsActive = isActive;
        }

    }
}