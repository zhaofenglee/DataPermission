using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace JS.Abp.DataPermission.Demos
{
    public class Demo : FullAuditedAggregateRoot<Guid>
    {
        [CanBeNull]
        public virtual string? Name { get; set; }

        [CanBeNull]
        public virtual string? DisplayName { get; set; }

        public Demo()
        {

        }

        public Demo(Guid id, string name, string displayName)
        {

            Id = id;
            Check.Length(name, nameof(name), DemoConsts.NameMaxLength, 0);
            Check.Length(displayName, nameof(displayName), DemoConsts.DisplayNameMaxLength, 0);
            Name = name;
            DisplayName = displayName;
        }

    }
}