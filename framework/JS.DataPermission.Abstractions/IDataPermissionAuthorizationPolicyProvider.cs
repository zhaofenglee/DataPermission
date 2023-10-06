using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDataPermissionAuthorizationPolicyProvider
{
    Task<TDto> CheckAsync<TDto>(TDto sourceDto);
    
    Task<List<TDto>> CheckListAsync<TDto>(IEnumerable<TDto> sourceListDto);
}