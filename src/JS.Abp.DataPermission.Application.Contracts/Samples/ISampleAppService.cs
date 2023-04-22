using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace JS.Abp.DataPermission.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
