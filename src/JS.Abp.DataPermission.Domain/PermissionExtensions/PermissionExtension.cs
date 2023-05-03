using JS.Abp.DataPermission.PermissionTypes;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public class PermissionExtension : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string ObjectName { get; set; }

        public virtual Guid RoleId { get; set; }

        public virtual Guid? ExcludedRoleId { get; set; }

        public virtual PermissionType PermissionType { get; set; }

        [NotNull]
        public virtual string LambdaString { get; set; }

        public virtual bool IsActive { get; set; }
        [CanBeNull]
        public virtual string? Description { get; set; }
        public PermissionExtension()
        {

        }

        public PermissionExtension(Guid id, string objectName, Guid roleId, PermissionType permissionType, string lambdaString, bool isActive,string description, Guid? excludedRoleId = null)
        {

            Id = id;
            Check.NotNull(objectName, nameof(objectName));
            Check.Length(objectName, nameof(objectName), PermissionExtensionConsts.ObjectNameMaxLength, 0);
            Check.NotNull(lambdaString, nameof(lambdaString));
            ObjectName = objectName;
            RoleId = roleId;
            PermissionType = permissionType;
            LambdaString = lambdaString;
            IsActive = isActive;
            Description = description;
            ExcludedRoleId = excludedRoleId;
        }

    }
}