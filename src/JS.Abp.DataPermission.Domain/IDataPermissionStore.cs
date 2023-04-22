using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JS.Abp.DataPermission;

public interface IDataPermissionStore
{
    List<DataPermissionResult> GetAll();
    Task<List<DataPermissionResult>> GetAllAsync();

    Task<bool> CheckPermissionAsync<T>(string id,T item);
}