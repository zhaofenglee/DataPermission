using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionTypes;

namespace JS.Abp.DataPermission;

public interface IDataPermissionStore
{
    List<DataPermissionResult> GetAll();
    Task<List<DataPermissionResult>> GetAllAsync();
    /// <summary>
    /// 查询当前数据是否有权限
    /// </summary>
    /// <param name="id"></param>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<bool> GetPermissionAsync<T>(string id,T item);
    /// <summary>
    /// 判断当前数据是否有权限
    /// </summary>
    /// <param name="item"></param>
    /// <param name="permissionType"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<bool> CheckPermissionAsync<T>(T item,PermissionType permissionType);
}