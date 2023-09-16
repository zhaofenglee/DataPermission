using System.Threading.Tasks;
using JetBrains.Annotations;
using JS.Abp.DataPermission.PermissionTypes;
using Volo.Abp.Application.Services;

namespace JS.Abp.DataPermission.Services;

public interface IPermissionApplicationService : IApplicationService
{
    /// <summary>
    /// 获取当前用户对当前数据权限
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="policyName">abp权限名称</param>
    /// <param name="permissionType">数据权限类型</param>
    /// <returns></returns>
    Task<GetPermissionResultDto> GetAsync(string id,string policyName ="",PermissionType permissionType=PermissionType.Update);
    /// <summary>
    /// 获取当前用户权限配置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<DataPermissionResult> GetDataFilterAsync(GetDataFilterInput input);
    /// <summary>
    /// 获取当前用户对象级数据权限
    /// </summary>
    /// <param name="input"></param>
    /// <returns>返回数据集为空或者是对应字段权限为true时则有权</returns>
    Task<DataPermissionItemDto> GetDataPermissionItemAsync(GetPermissionItemInput input);
}