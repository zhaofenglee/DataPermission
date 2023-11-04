using Volo.Abp.Identity;

namespace JS.Abp.DataPermission;

public class IdentityUserWithLevel
{
    public int Level { get; set; }
    public IdentityUser IdentityUser { get; set; }
}