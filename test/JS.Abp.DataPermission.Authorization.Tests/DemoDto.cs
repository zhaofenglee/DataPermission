using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace JS.Abp.DataPermission.Authorization.Tests
{
    public class DemoDto
    {
        [Permission("MyPermission5")]
        public string? Name { get; set; }
        [Permission("UndefinedPermission")]
        public string? DisplayName { get; set; }
    }
}