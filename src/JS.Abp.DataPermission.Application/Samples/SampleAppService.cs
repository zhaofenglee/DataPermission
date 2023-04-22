using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace JS.Abp.DataPermission.Samples;

public class SampleAppService : DataPermissionAppService, ISampleAppService
{
    public Task<SampleDto> GetAsync()
    {
        return Task.FromResult(
            new SampleDto
            {
                Value = 42
            }
        );
    }

    [Authorize]
    public Task<SampleDto> GetAuthorizedAsync()
    {
        CheckDataPermission();
        return Task.FromResult(
            new SampleDto
            {
                Value = 42
            }
        );
    }
}
