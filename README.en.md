# DataPermission

---

<div align="center">
<p><strong><a href="README.en.md">English</a> | <a href="README.md">简体中文</a> </strong></p>
</div>

---
## New Features
* Added field-level data permission functionality
* Added a list of entity data permissions and merged row-level and field-level data permissions into the same detail page.


## 7.3.1
#### 1. Added the ability to copy data permissions.

## 7.2.3
#### 1. Added organizational permission controls.
The expression is configured as "x.CreatorId == 'OrganizationUser'" will automatically search for all subordinates in the current organization.

* Supports rewriting the method to obtain organization members (default to ABP organization).
````csharp
context.Services.Replace(ServiceDescriptor.Singleton<IOrganizationStore, MyOrganizationStore>());

public class MyOrganizationStore:OrganizationStore
{
public MyOrganizationStore(IIdentityUserRepository identityUserRepository, IOrganizationUnitRepository organizationUnitRepository) : base(identityUserRepository, organizationUnitRepository)
{

    }
    public override async Task<List<string>> GetMenberInOrganizationUnitAsync(Guid id)
    {
        List<string> result = new List<string>();
        result.Add(id.ToString());
        return result;
    }


}
`````

## 7.2.1
1. The DataPermissionExtensions has added a description field, which can be used to add commentary information.
2. The DataPermissionStore has added permission evaluation methods, which can be used according to specific business scenarios.

## Implementation Principle

### Row-level Data Authorization
* Row-level query authorization is achieved by filtering through warehouse data query conditions.
* Row-level authorization for modification and deletion is achieved by querying for a single row object in the warehouse and then checking for permission. If there is no permission, an exception is thrown or handled in the UI (if there is a better method, please feel free to suggest it).
* By setting roles and corresponding entities, custom Lambda expressions can be used to filter query data.
* By setting roles and corresponding entities, modification and deletion can be achieved, and permission must be checked after querying.
* By setting roles and corresponding entities, creation can be achieved. Currently, authorization for new additions must be checked before creation. If there is no permission, an exception is thrown. For example:
  “var checkPermission = await dataPermissionStore.CheckPermissionAsync(item, PermissionType.Create);”

### Field-Level Data Permissions
* Field-level data permissions are judged based on the maintained field names and corresponding role permissions. To use field-level data permissions, front-end and back-end code modifications are required.

## Cache
* Currently, the cache time is 10 minutes, and the cache will be automatically cleared if the "PermissionExtension" object changes.
* The row-level data permission cache time is 10 minutes, and the permissions will be overwritten after they are re-queried when changes occur.
* The default cache time can be modified through configuration.
````csharp
  Configure<DataPermissionOptions>(options =>
  {
  options.CacheExpirationTime = 60;
  });
````


## Preparations

### 1. Install NuGet packages.
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
*(Optional)  JS.Abp.DataPermission.Web

### 2. Add "dependencies" attribute to configure modules.
* [DependsOn(typeof(DataPermissionApplicationModule))]
* [DependsOn(typeof(DataPermissionApplicationContractsModule))]
* [DependsOn(typeof(DataPermissionDomainModule))]
* [DependsOn(typeof(DataPermissionDomainSharedModule))]
* [DependsOn(typeof(DataPermissionEntityFrameworkCoreModule))] OR [DependsOn(typeof(DataPermissionMongoDbModule))]
* [DependsOn(typeof(DataPermissionHttpApiModule))]
* [DependsOn(typeof(DataPermissionHttpApiClientModule))]


*(Optional)  [DependsOn(typeof(DataPermissionBlazorModule))]
*(Optional)  [DependsOn(typeof(DataPermissionBlazorServerModule))]
*(Optional)  [DependsOn(typeof(DataPermissionBlazorWebAssemblyModule))]
*(Optional)  [DependsOn(typeof(DataPermissionWebModule))]

### *If using MongoDB, the following steps can be ignored.
### 3. Add `builder.ConfigureDataPermission();` to your DbContext.

### 4. Add EF Core migrations and update the database.
Open a command-line terminal in the directory of YourProject.EntityFrameworkCore project and enter the following command:

````bash
> dotnet ef migrations add Added_DataPermission
````
````bash
> dotnet ef database update
````

## Row-Level Data Authorization

* The following code has been implemented in the Demo solution. Please refer to the Demo for specific usage.

### 1. Configure the entity name, type, and corresponding Lambda expression for row-level data authorization.
* The query expression filters the original query expression by default. The expression format needs to be noted, such as "x.Name != "Abc"", where x is the fixed format, Name is the entity property, and Abc is the filter condition. The example means that the current role is only allowed to query data where Name is not equal to Abc.
* The creation, modification, and deletion expressions need to be noted in the format, such as "x.Name == "Abc"", where x is the fixed format, Name is the entity property, and Abc is the filter condition. The example means that the current role is only allowed to create, modify, and delete data where Name is equal to Abc.
* Data authorization control has provided a Blazor page, please implement it yourself for other front-end frameworks.

![PermissionControl](/docs/images/PermissionControl.gif)

### 2. Add the following code to the repository that requires permission control.
````csharp
 protected IDataPermissionStore dataPermissionStore => LazyServiceProvider.LazyGetRequiredService<IDataPermissionStore>();
````
### 3. Add `query = dataPermissionStore.EntityFilter(query);//add` before the filtering conditions.
````csharp
protected virtual IQueryable<Demo> ApplyFilter(
            IQueryable<Demo> query,
            string filterText,
            string name = null,
            string displayName = null)
        {
            query = dataPermissionStore.EntityFilter(query)//add
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name.Contains(filterText) || e.DisplayName.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
                    .WhereIf(!string.IsNullOrWhiteSpace(displayName), e => e.DisplayName.Contains(displayName));
        }
````
### 4. After performing the above operations, the query filtering function can be used.

### 5. If permission control for modification and deletion is needed, there are two implementation methods.
#### 5.1. It is necessary to add `await dataPermissionStore.GetPermissionAsync(id.ToString(), item);` before the method of querying a single data.
* This step is to check whether the current user has the permission to modify and delete the data, which is used to determine whether modification and deletion operations can be performed later.
````csharp
 public async Task<Demo> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            query = DataPermissionExtensions.EntityFilter(query,  await dataPermissionStore.GetAllAsync());//add
            var item =  await query.FirstOrDefaultAsync(e => e.Id == id, GetCancellationToken(cancellationToken));
            await dataPermissionStore.GetPermissionAsync(id.ToString(), item);//add
            return item;
        }
````

![Demo](/docs/images/Demo.gif)

* Introduce the "IPermissionApplicationService" service for services that require permission control.
````csharp
 protected IPermissionApplicationService permissionApplicationService => LazyServiceProvider.LazyGetRequiredService<IPermissionApplicationService>();
````
* Add `await permissionApplicationService.CheckPermissionAsync(id.ToString(), "abp permission name", PermissionType.Update);` before the methods that require permission control.
* The rule for permission judgment is: if the current user has no "abp permission" permission, return false directly. Then, check if there is a row permission. If not, the abp permission name can be passed in as null to skip.
* Whether to control on the UI or the backend can be chosen according to the actual situation. Below is an example using Blazor.
````csharp
var demo = await DemosAppService.GetAsync(input.Id);
CanEditDemo =  PermissionApplicationService.GetAsync(demo.Id.ToString(),DataPermissionPermissions.Demos.Edit, PermissionType.Update).Result.IsGranted;//add
````

#### 5.2 Direct judgement on the service
* This step is to judge whether the incoming object has permission. If it does not have permission, an exception will be thrown. It is important to note that the object Name passed in must be consistent with the entity name.
````csharp
 public async Task<Demo> UpdateAsync(Guid id, UpdateDemoDto input, CancellationToken cancellationToken = default)
        {
            var item = await GetAsync(id, cancellationToken);
            var checkPermission =await dataPermissionStore.CheckPermissionAsync(item, PermissionType.Update);//add
            //Todo:无权的操作
            ObjectMapper.Map(input, item);
            await ValidateAsync(item);
            await Repository.UpdateAsync(item, autoSave: true);
            return item;
        }
````


### 6. Create permission control methods to check the incoming object before inserting it into the database.
````csharp
//add permission check
if (!dataPermissionStore.CheckPermission(demo, PermissionType.Create))
            {
                throw new UserFriendlyException(
                    "The current user does not have permission to create data!"
                );
            }
````

### 7. Return the permissions corresponding to the data in the service.
````csharp
//创建一个数据权限的Dto
public class PermissionItemDto
{
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }
}
//在查询返回的Dto中加入
public PermissionItemDto Permission { get; set; } = new PermissionItemDto();
//AutoMapper中需要先忽略Permission
 .ForMember(dest => dest.Permission, opt => opt.Ignore());

//在查询结果返回前把权限取出
 var dtos = items.Select(queryResultItem =>
            {
                var dto = ObjectMapper.Map<Demo, DemoDto>(queryResultItem);
                dto.Permission = ObjectMapper.Map<PermissionCacheItem, PermissionItemDto>(dataPermissionStore.GetPermissionAsync(queryResultItem).Result);//add
                return dto;
            }).ToList();
````

## Field-level Data Permissions
* The following code has already been implemented in the demo solution. Please refer to the demo for specific usage instructions.

### 1. Configure the entity name, type, corresponding field name, and corresponding permissions that need to be controlled in field-level data permissions.
![img](/docs/images/20230901200918.png)

### 2. Check if there is permission in the warehouse or data writing service before writing.
````csharp 
 protected IDataPermissionItemStore dataPermissionItemStore => LazyServiceProvider.LazyGetRequiredService<IDataPermissionItemStore>();//字段级数据权限
 
    if (dataPermissionItemStore.CheckUpdate(nameof(Demo), "Name"))
                demo.Name = name;
   if (dataPermissionItemStore.CheckUpdate(nameof(Demo), "DisplayName"))
                demo.DisplayName = displayName;
 
````
### 3. Control on the frontend. Here is an example using Blazor.

````csharp
@inject IPermissionApplicationService PermissionApplicationService
      private DataPermissionItemDto DataPermissionItem { get; set; }
      //这里把字段级权限获取，再传递给前端，前端根据权限判断是否显示编辑
            //需要注意判断是否有权限规则如下参考：
            //if (!DataPermissionItem.PermissionItems.Any(x => x.TargetType == "DisplayName")||DataPermissionItem.PermissionItems.FirstOrDefault(x => x.TargetType == "DisplayName").CanEdit)
            DataPermissionItem =
                await PermissionApplicationService.GetDataPermissionItemAsync(new GetPermissionItemInput()
                {
                    ObjectName = "Demo",
                });
````

### 4. Control in services, and replace unauthorized properties with default values when querying or exporting.
#### 4.1. First configure the DTO.
````csharp
 public class DemoDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string? Name { get; set; }
        [PermissionVerifier("Demo", "DisplayName")]
        public string? DisplayName { get; set; }
        public RowPermissionItemDto Permission { get; set; } = new RowPermissionItemDto();

        public string ConcurrencyStamp { get; set; }
    }
````
#### 4.2. Validate permissions when querying or exporting data, and replace the dataset with default values if no permission.
````csharp
public virtual async Task<DemoDto> GetAsync(Guid id)
        {
            var demo = await _demoRepository.GetAsync(id);
            var item =  ObjectMapper.Map<Demo, DemoDto>(demo);
            await dataPermissionItemStore.CheckAsync(item);//add
            //await dataPermissionItemStore.CheckListAsync(dtos);//如果是列表需要使用这个方法
            item.Permission = ObjectMapper.Map<PermissionCacheItem, RowPermissionItemDto>( await dataPermissionStore.GetPermissionAsync(demo));
            return item;
        }
`````
## Samples

See the [sample projects](https://github.com/zhaofenglee/DataPermission/tree/master/host/JS.Abp.DataPermission.Blazor.Server.Host)

## Online demo
https://rxsoftware.cn/

账号：data001/data002
密码：1q2w3E*

## Thanks

####  [Jetbrains](https://www.jetbrains.com/)

![JetBrains Logo (Main) logo](https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.svg)

Thank you for providing free IDE support for this project. ([License](https://jb.gg/OpenSourceSupport))