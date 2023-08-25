using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.PermissionTypes;

namespace JS.Abp.DataPermission;

public interface IDataPermissionStore
{
    /// <summary>
    /// 根据当前用户过滤实体
    /// </summary>
    /// <param name="query"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    IQueryable<TEntity> EntityFilter<TEntity>(IQueryable<TEntity> query);
    /// <summary>
    /// 获取所有权限控制规则，带缓存
    /// </summary>
    /// <returns></returns>
    List<DataPermissionResult> GetAll();
    /// <summary>
    /// 获取所有权限控制规则，带缓存
    /// </summary>
    /// <returns></returns>
    Task<List<DataPermissionResult>> GetAllAsync();
    /// <summary>
    /// 查询当前数据是否有权限
    /// </summary>
    /// <param name="id"></param>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [Obsolete("GetPermissionAsync is Obsolete,Use GetPermissionByIdAsync")]
    Task<bool> GetPermissionAsync<T>(string id,T item);
    /// <summary>
    /// 查询当前数据是否有权限
    /// </summary>
    /// <param name="id"></param>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    PermissionCacheItem GetPermissionById<T>(string id,T item);
    /// <summary>
    /// 查询当前数据是否有权限
    /// </summary>
    /// <param name="id"></param>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<PermissionCacheItem> GetPermissionByIdAsync<T>(string id,T item);
    /// <summary>
    /// 获取当前数据所有权限,entity需包含Id
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    PermissionCacheItem GetPermission<TEntity>(TEntity entity);
    /// <summary>
    /// 获取当前数据所有权限,entity需包含Id
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    Task<PermissionCacheItem> GetPermissionAsync<TEntity>(TEntity entity);
    /// <summary>
    /// 判断当前数据是否有权限
    /// </summary>
    /// <param name="item"></param>
    /// <param name="permissionType"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    bool CheckPermission<T>(T item,PermissionType permissionType);
    /// <summary>
    /// 判断当前数据是否有权限
    /// </summary>
    /// <param name="item"></param>
    /// <param name="permissionType"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<bool> CheckPermissionAsync<T>(T item,PermissionType permissionType);
}