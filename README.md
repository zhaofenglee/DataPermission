# 数据权限

## 实现原理
* 行数据查询权限是通过仓储对于数据查询进行过滤
* 行数据权限修改和删除是通过仓储查询单个行对象是判断是否有权限，应该先查询一次数据，在判断是否有权限，如果没有权限则抛出异常或在UI进行处理
* 通过设置 角色 和对应实体 实现查询可以根据自定义Lambda表达式过滤查询数据
* 通过设置 角色 和对应实体 实现修改，删除
* 目前不支持新增，新增的数据权限建议使用内置的数据权限

## 缓存
* 目前缓存时间为 10 分钟，如果“PermissionExtension”对象变更时会自动清除
* 行级数据权限缓存时间为 10 分钟，权限变更后重新查询权限会进行覆盖

## 准备工作

### 1.安装 NuGet packages.
* JS.Abp.DataPermission.Application
* JS.Abp.DataPermission.Application.Contracts
* JS.Abp.DataPermission.Domain
* JS.Abp.DataPermission.Domain.Shared
* JS.Abp.DataPermission.EntityFrameworkCore
* JS.Abp.DataPermission.HttpApi
* JS.Abp.DataPermission.HttpApi.Client

*(Optional)  JS.Abp.DataPermission.Blazor
*(Optional)  JS.Abp.DataPermission.Blazor.Server
*(Optional)  JS.Abp.DataPermission.Blazor.WebAssembly

### 2.添加“依赖”属性以配置模块
* [DependsOn(typeof(DataPermissionApplicationModule))]
* [DependsOn(typeof(DataPermissionApplicationContractsModule))]
* [DependsOn(typeof(DataPermissionDomainModule))]
* [DependsOn(typeof(DataPermissionDomainSharedModule))]
* [DependsOn(typeof(DataPermissionEntityFrameworkCoreModule))]
* [DependsOn(typeof(DataPermissionHttpApiModule))]
* [DependsOn(typeof(DataPermissionHttpApiClientModule))]


*(Optional)  [DependsOn(typeof(DataPermissionBlazorModule))]
*(Optional)  [DependsOn(typeof(DataPermissionBlazorServerModule))]
*(Optional)  [DependsOn(typeof(DataPermissionBlazorWebAssemblyModule))]

### 3. 在你的Dbcontext添加 ` builder.ConfigureDataPermission();` 

### 4. 添加 EF Core 迁移并更新数据库
在 YourProject.EntityFrameworkCore 项目的目录中打开命令行终端，然后键入以下命令：

````bash
> dotnet ef migrations add Added_DataPermission
````
````bash
> dotnet ef database update
````

## 如何使用
* 以下是参考Demo的使用方法，具体使用方法请参考Demo
### 1.在需要进行权限控制的仓储加入下述代码
````csharp
 protected IDataPermissionStore dataPermissionStore => LazyServiceProvider.LazyGetRequiredService<IDataPermissionStore>();
````
### 2.在过滤条件前添加 `query = DataPermissionExtensions.EntityFilter(query,  dataPermissionStore.GetAll());`
````csharp
protected virtual IQueryable<Demo> ApplyFilter(
            IQueryable<Demo> query,
            string filterText,
            string name = null,
            string displayName = null)
        {
            query = DataPermissionExtensions.EntityFilter(query,  dataPermissionStore.GetAll());//add
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name.Contains(filterText) || e.DisplayName.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
                    .WhereIf(!string.IsNullOrWhiteSpace(displayName), e => e.DisplayName.Contains(displayName));
        }
````
### 3.进行上述操作后查询过滤功能就可以使用了，如果需要进行修改和删除的权限控制，需要在查询单条数据方法前加入 ` var checkPermission =await dataPermissionStore.CheckPermissionAsync(id.ToString(), item);`
* 这一步是为了判断当前用户是否有该数据修改和删除权限
````csharp
 public async Task<Demo> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            query = DataPermissionExtensions.EntityFilter(query,  await dataPermissionStore.GetAllAsync());//add
            var item =  await query.FirstOrDefaultAsync(e => e.Id == id, GetCancellationToken(cancellationToken));
            var checkPermission =await dataPermissionStore.CheckPermissionAsync(id.ToString(), item);//add
            return item;
        }
````

### 4.在需要进行权限控制的服务引入服务“IPermissionApplicationService”
````csharp
 protected IPermissionApplicationService permissionApplicationService => LazyServiceProvider.LazyGetRequiredService<IPermissionApplicationService>();
````
## 5.在需要进行权限控制的方法前加入 `await permissionApplicationService.CheckPermissionAsync(id.ToString(), “abp权限名称”, PermissionType.Update);`
* 权限判断规则是：如果当前用户是否有“abp权限”权限，没有直接为false，再判断是否有行权限
* 在UI上控制还是在服务上进行控制可以根据实际情况进行选择，以下是以Blazor为例
````csharp
var demo = await DemosAppService.GetAsync(input.Id);
CanEditDemo =  PermissionApplicationService.GetAsync(demo.Id.ToString(),DataPermissionPermissions.Demos.Edit, PermissionType.Update).Result.IsGranted;//add
````

## Samples

See the [sample projects](https://github.com/zhaofenglee/DataPermission/tree/master/host/JS.Abp.DataPermission.Blazor.Server.Host)
