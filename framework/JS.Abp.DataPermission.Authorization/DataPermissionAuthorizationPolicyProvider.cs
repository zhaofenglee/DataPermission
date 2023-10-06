using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;

public class DataPermissionAuthorizationPolicyProvider:IDataPermissionAuthorizationPolicyProvider
{
    private readonly IPermissionChecker _permissionChecker;
    public DataPermissionAuthorizationPolicyProvider(IPermissionChecker permissionChecker)
    {
        _permissionChecker = permissionChecker;
    }

    public virtual async Task<TDto> CheckAsync<TDto>(TDto sourceDto)
    {
        var type = typeof(TDto);
        PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        DataColumnInfo columnInfo = new();
        var list = props.Select(p => 
        {
            var targetType = p.GetAttribute<PermissionAttribute>();
            if (targetType == null)
                return null;
            return new DataColumnInfo()
            {
                Property = p,
                PermissionName = targetType.PermissionName,
            };
        } ).Where(_ => _ != null);
        if(list.Any())
        {
            foreach(var info in list)
            {
                var checkAuthorizationResult = await _permissionChecker
                    .IsGrantedAsync(info.PermissionName);
                if (!checkAuthorizationResult)
                {
                    if (info.Property.PropertyType.IsGenericType && info.Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        // If it's a nullable type, set the property to null
                        info.Property.SetValue(sourceDto, null);
                    }
                    else if (info.Property.PropertyType.IsValueType)
                    {
                        // If it's a value type, set the property to its default value
                        info.Property.SetValue(sourceDto, Activator.CreateInstance(info.Property.PropertyType));
                    }
                    else
                    {
                        // For reference types, you may choose to set it to null or handle differently
                        info.Property.SetValue(sourceDto, null);
                    }
                    continue;
                }
            }
        }
        return sourceDto;

    }

    public virtual async Task<List<TDto>> CheckListAsync<TDto>(IEnumerable<TDto> sourceListDto)
    {
        var listDto = sourceListDto as TDto[] ?? sourceListDto.ToArray();
        foreach (var dto in listDto)
        {
            await CheckAsync(dto);
        }
        return listDto.ToList();
    }

    private class DataColumnInfo
    {
        public string PermissionName { get; set; }

        public PropertyInfo Property { get; set; }
    }
}