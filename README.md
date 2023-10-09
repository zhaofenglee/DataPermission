# 数据权限

---

<div align="center">
<p><strong><a href="README.en.md">English</a> | <a href="README.md">简体中文</a> </strong></p>
</div>

---

## 7.4.0更新说明
* 增加基于Permission数据权限
````csharp
//前端请参考Abp文档https://docs.abp.io/en/abp/latest/Authorization
//后端使用方法参考:AuthorizationPolicyProvider_Test

//1.在需要输出Dto上加入PermissionAttribute
public class DemoDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string? Name { get; set; }
        [Permission(DataPermissionPermissions.Demos.Read)]//Add
        public string? DisplayName { get; set; }
        public RowPermissionItemDto Permission { get; set; } = new RowPermissionItemDto();

        public string ConcurrencyStamp { get; set; }
    }
//2.在需要控制输出的方法中使用，若无权，会替换成默认值输出
   private readonly IDataPermissionAuthorizationPolicyProvider _dataPermissionAuthorizationPolicyProvider;
   await _dataPermissionAuthorizationPolicyProvider.CheckListAsync(dtos);
````
## 7.3.3更新说明
* 增加字段级数据权限功能
* 增加实体数据权限列表，并且把行级和字段级数据权限合并在同一个明细页面
(如果希望把行级和字段级菜单单独添加可以参考DataPermissionMenuContributor自行添加)
![img](/docs/images/20230915215545.png)



## 7.3.1更新说明
#### 1.增加复制数据权限行功能

## 7.2.3更新说明
#### 1.增加基于组织权限控制
表达式配置为”x.CreatorId=="OrganizationUser"“ 会自动寻找当前组织所有下级成员

* 支持重写获取组织成员方法（默认取abp组织）
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

## 7.2.1更新说明
#### 1.DataPermissionExtensions增加了描述字段，可以添加备注信息
#### 2.DataPermissionStore增加了权限判断方法，可以根据自己业务场景使用

## 实现原理
### 行级数据权限
* 行数据查询权限是通过仓储数据查询条件进行过滤
* 行数据权限修改和删除是通过仓储查询单个行对象再判断是否有权限，如果没有权限则抛出异常或在UI进行处理（有更好的方法也欢迎大家提出）
* 通过设置 角色 和对应实体 实现查询可以根据自定义Lambda表达式过滤查询数据
* 通过设置 角色 和对应实体 实现修改，删除，需要查询后再判断是否有权限
* 通过设置 角色 和对应实体 实现创建，目前新增需要在创建之前进行权限判断，如果没有权限则抛出异常，如：
  “var checkPermission = await dataPermissionStore.CheckPermissionAsync(item, PermissionType.Create);”

### 字段级数据权限
* 字段数据权限是根据维护的字段名称和对应角色的权限进行判断，使用字段级数据权限需要修改前端和后端代码共同实现

## 缓存
* 目前缓存时间为 10 分钟，如果“PermissionExtension”对象变更时会自动清除
* 行级数据权限缓存时间为 10 分钟，权限变更后重新查询权限会进行覆盖
* 可以通过配置方式修改默认缓存时间
````csharp
  Configure<DataPermissionOptions>(options =>
  {
  options.CacheExpirationTime = 60;
  });
````


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
*(Optional)  JS.Abp.DataPermission.Web

### 2.添加“依赖”属性以配置模块
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

### *若使用MongoDb，以下步骤可以忽略
### 3. 在你的Dbcontext添加 ` builder.ConfigureDataPermission();` 

### 4. 添加 EF Core 迁移并更新数据库
在 YourProject.EntityFrameworkCore 项目的目录中打开命令行终端，然后键入以下命令：

````bash
> dotnet ef migrations add Added_DataPermission
````
````bash
> dotnet ef database update
````

## 行级数据权限
* 以下代码已经在Demo方案实现，具体使用方法请参考Demo

### 1.在行级数据权限配置需要控制的实体名称，类型，及对应的Lambda表达式
* 查询表达式默认是在原有的查询表达式上进行过滤，表达式需要注意格式，如：“x.Name != "Abc"”，x.为固定格式，Name为实体属性，Abc为过滤条件，实例的含义是当前角色仅允许查询Name不等于Abc的数据
* 创建，修改和删除表达式需要注意格式，如：“x.Name == "Abc"”，x.为固定格式，Name为实体属性，Abc为过滤条件，实例的含义是当前角色仅允许创建，修改和删除Name等于Abc的数据
* 数据权限控制已提供Blazor页面，其他前端框架请自行实现

![PermissionControl](/docs/images/PermissionControl.gif)

### 2.在需要进行权限控制的仓储加入下述代码
````csharp
 protected IDataPermissionStore dataPermissionStore => LazyServiceProvider.LazyGetRequiredService<IDataPermissionStore>();
````
### 3.在过滤条件前添加 `query = DataPermissionExtensions.EntityFilter(query,  dataPermissionStore.GetAll());`
* abp7.2 可以直接使用“query = dataPermissionStore.EntityFilter(query);//add”
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
### 4.进行上述操作后查询过滤功能就可以使用

### 5.如果需要进行修改和删除的权限控制，有两个实现方法
#### 5.1需要在查询单条数据方法前加入 ` await dataPermissionStore.GetPermissionAsync(id.ToString(), item);`
* 这一步是为了查询当前用户是否有该数据修改和删除权限，用于后面判断是否可以进行修改和删除操作
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

* 在需要进行权限控制的服务引入服务“IPermissionApplicationService”
````csharp
 protected IPermissionApplicationService permissionApplicationService => LazyServiceProvider.LazyGetRequiredService<IPermissionApplicationService>();
````
* 在需要进行权限控制的方法前加入 `await permissionApplicationService.CheckPermissionAsync(id.ToString(), “abp权限名称”, PermissionType.Update);`
* 权限判断规则是：如果当前用户是否有“abp权限”权限，没有直接为false，再判断是否有行权限，若无须判断abp权限名称传入空值可跳过
* 在UI上控制还是在服务上进行控制可以根据实际情况进行选择，以下是以Blazor为例
````csharp
var demo = await DemosAppService.GetAsync(input.Id);
CanEditDemo =  PermissionApplicationService.GetAsync(demo.Id.ToString(),DataPermissionPermissions.Demos.Edit, PermissionType.Update).Result.IsGranted;//add
````

#### 5.2直接在服务上进行判断 
* 这一步是把传入的对象判断是否有权限，如果没有权限则抛出异常，需要注意是传入的对象Name必须和实体名字一致
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


### 6.创建权限控制方法，可以在插入数据库前对传入对象进行判断
````csharp
//add permission check
if (!dataPermissionStore.CheckPermission(demo, PermissionType.Create))
            {
                throw new UserFriendlyException(
                    "The current user does not have permission to create data!"
                );
            }
````

### 7.在服务上返回数据对应的权限
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

## 字段级数据权限
* 以下代码已经在Demo方案实现，具体使用方法请参考Demo

### 1.在字段级数据权限配置需要控制的实体名称，类型，对应字段名称及对应的权限
![img](/docs/images/20230901200918.png)

### 2.在仓储或者数据写入服务中判断是否有权限，有权限再写入
````csharp 
 protected IDataPermissionItemStore dataPermissionItemStore => LazyServiceProvider.LazyGetRequiredService<IDataPermissionItemStore>();//字段级数据权限
 
    if (dataPermissionItemStore.CheckUpdate(nameof(Demo), "Name"))
                demo.Name = name;
   if (dataPermissionItemStore.CheckUpdate(nameof(Demo), "DisplayName"))
                demo.DisplayName = displayName;
 
````
### 3.在前端上进行控制，以下是以Blazor为例

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

### 4.在服务上进行控制，如查询或导出时，把没有权限属性替换成默认值
#### 4.1.首先配置Dto
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
#### 4.2.在查询或导出时校验权限，无权时会把数据集替换成默认值
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

## 在线演示
https://rxsoftware.cn/

账号：data001/data002
密码：1q2w3E*

## 感谢名单

####  [Jetbrains](https://www.jetbrains.com/)

![JetBrains Logo (Main) logo](https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.svg)

感谢提供免费IDE支持此项目 ([License](https://jb.gg/OpenSourceSupport))