using System.ComponentModel.DataAnnotations;

namespace JS.Abp.DataPermission.Services;

public class GetPermissionItemInput
{
    [Required]
    public string ObjectName { get; set; }
}