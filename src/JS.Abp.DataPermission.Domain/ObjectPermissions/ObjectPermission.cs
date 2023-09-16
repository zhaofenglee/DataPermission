using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public class ObjectPermission : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string ObjectName { get; set; }

        [CanBeNull]
        public virtual string? Description { get; set; }

        public ObjectPermission()
        {

        }

        public ObjectPermission(Guid id, string objectName, string description)
        {

            Id = id;
            Check.NotNull(objectName, nameof(objectName));
            Check.Length(objectName, nameof(objectName), ObjectPermissionConsts.ObjectNameMaxLength, 0);
            ObjectName = objectName;
            Description = description;
        }

    }
}