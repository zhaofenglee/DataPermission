using System;
using Volo.Abp.Application.Dtos;

namespace JS.Abp.DataPermission.PermissionExtensions;

public class PermissionRoleDto:EntityDto<Guid>
{
    public string Name { get; set; }
}