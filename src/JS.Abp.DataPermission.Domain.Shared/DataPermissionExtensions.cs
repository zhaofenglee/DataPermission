using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using JS.Abp.DataPermission.PermissionTypes;

namespace JS.Abp.DataPermission;

public static class DataPermissionExtensions
{
    /// <summary>
    /// 实体查询过滤
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="dataPermissions"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static  IQueryable<TEntity> EntityFilter<TEntity>(IQueryable<TEntity> entity,List<DataPermissionResult> dataPermissions)
    {
        var query = entity;
        if (dataPermissions!=null&&dataPermissions.Count>0)
        {
            foreach (var dataPermission in dataPermissions.Where(c=>c.PermissionType==PermissionType.Read&&c.ObjectName==typeof(TEntity).Name))
            {
                query = Filter(query, dataPermission);
            }
            return query;
        }
        else
        {
            return query;
        }
        
    }
    /// <summary>
    /// 检查实体是否有权限
    /// </summary>
    /// <param name="item"></param>
    /// <param name="dataPermission"></param>
    /// <param name="permissionType"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool CheckItem<T>(T item, DataPermissionResult dataPermission,PermissionType permissionType)
    {
        var func = (Func<T, bool>)DynamicExpressionParser.ParseLambda(
            typeof(Func<T, bool>),
            new[]
            {
                Expression.Parameter(typeof(T), "x")
            },
            null,
            dataPermission.LambdaString
        ).Compile();
        return func(item);
    }
    private static  IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> entity,DataPermissionResult dataPermission)
    {
        return entity.Where(CreateFilterExpression(typeof(TEntity), dataPermission));
    }
    public static bool CheckItem<T>(T item, DataPermissionResult dataPermission)
    {
        return CheckItem(item, dataPermission, PermissionType.UpdateAndDelete);
    }
    
    private static LambdaExpression CreateFilterExpression(Type entityType,DataPermissionResult dataPermission)
    {
        dataPermission.LambdaString = dataPermission.LambdaString.Replace("CurrentUser", dataPermission.UserId.ToString());
        //string lambdaString = "x.Name != \"test1\" && x.Name != \"test2\" || x.Name == \"test\"";
        var parameter = Expression.Parameter(entityType, "x");
        var lambda = DynamicExpressionParser.ParseLambda(new[] { parameter },null, dataPermission.LambdaString);
       
        return lambda;
        // var parameter = Expression.Parameter(entityType, "e");
        // var filedname = Expression.Property(parameter, dataPermission.FieldName);
        // var activeFilter = dataPermission.IsEqual?Expression.Equal(filedname, Expression.Constant(dataPermission.FieldValue)): Expression.NotEqual(filedname, Expression.Constant(dataPermission.FieldValue));
        // var lambda = Expression.Lambda(activeFilter, parameter);
        // return lambda;
    }
}