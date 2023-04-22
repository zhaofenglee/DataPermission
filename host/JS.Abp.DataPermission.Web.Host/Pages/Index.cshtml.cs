using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace JS.Abp.DataPermission.Pages;

public class IndexModel : DataPermissionPageModel
{
    public void OnGet()
    {

    }

    public async Task OnPostLoginAsync()
    {
        await HttpContext.ChallengeAsync("oidc");
    }
}
