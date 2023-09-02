using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.PermissionTypes;

namespace JS.Abp.DataPermission;

/// <summary>
/// 字段级数据权限
/// </summary>
public interface IDataPermissionItemStore
{
    /// <summary>
    /// 判断当前字段是否有权限
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="targetName"></param>
    /// <param name="permissionType"></param>
    /// <returns></returns>
    bool CheckPermission(string objectName, string targetName, PermissionType permissionType);
    /// <summary>
    /// 检查是否有创建权限
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="targetName"></param>
    /// <returns></returns>
    bool CheckCreate(string objectName, string targetName);
    /// <summary>
    /// 检查是否有读取权限
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="targetName"></param>
    /// <returns></returns>
    bool CheckRead(string objectName, string targetName);
    /// <summary>
    /// 检查是否有更新权限
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="targetName"></param>
    /// <returns></returns>
    bool CheckUpdate(string objectName, string targetName);
    /// <summary>
    /// 检查是否有删除权限
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="targetName"></param>
    /// <returns></returns>
    bool CheckDelete(string objectName, string targetName);
    /// <summary>
    /// 获取当前对象权限，带缓存
    /// </summary>
    /// <param name="objectName"></param>
    /// <returns></returns>
    Task<List<PermissionItemResult>> GetListAsync(string objectName);
  
}