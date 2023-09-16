using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace JS.Abp.DataPermission.PermissionItems
{
    public class PermissionItemsAppServiceTests : DataPermissionApplicationTestBase
    {
        private readonly IPermissionItemsAppService _permissionItemsAppService;
        private readonly IRepository<PermissionItem, Guid> _permissionItemRepository;

        public PermissionItemsAppServiceTests()
        {
            _permissionItemsAppService = GetRequiredService<IPermissionItemsAppService>();
            _permissionItemRepository = GetRequiredService<IRepository<PermissionItem, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _permissionItemsAppService.GetListAsync(new GetPermissionItemsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("b3f400c1-6974-4c36-9275-05d472458284")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("721fb0ec-9705-44c3-ac98-58eb377a7597")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _permissionItemsAppService.GetAsync(Guid.Parse("b3f400c1-6974-4c36-9275-05d472458284"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("b3f400c1-6974-4c36-9275-05d472458284"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new PermissionItemCreateDto
            {
                ObjectName = "318aae3754894be1881de86ab54b37f173e707fdd04b4dd8a6bf305ea4631ba4e978ad6beeeb4b19afab224c7955221da04e0a645e21422dac2930872a18097d",
                Description = "ad0cc41aad864a74b4dbb3e8821320a771ad944ffe6b4a9283b4ae17cb5",
                TargetType = "2b6ac13b4167448c942d0f71bad021e7f16922911f6c4f78bb54bcfe238b9360e74b1036fb5344a498442d7229c9bb502a1969d355544fd98519282ffe39b220",
                CanRead = true,
                CanCreate = true,
                CanEdit = true,
                CanDelete = true
            };

            // Act
            var serviceResult = await _permissionItemsAppService.CreateAsync(input);

            // Assert
            var result = await _permissionItemRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.ObjectName.ShouldBe("318aae3754894be1881de86ab54b37f173e707fdd04b4dd8a6bf305ea4631ba4e978ad6beeeb4b19afab224c7955221da04e0a645e21422dac2930872a18097d");
            result.Description.ShouldBe("ad0cc41aad864a74b4dbb3e8821320a771ad944ffe6b4a9283b4ae17cb5");
            result.TargetType.ShouldBe("2b6ac13b4167448c942d0f71bad021e7f16922911f6c4f78bb54bcfe238b9360e74b1036fb5344a498442d7229c9bb502a1969d355544fd98519282ffe39b220");
            result.CanRead.ShouldBe(true);
            result.CanCreate.ShouldBe(true);
            result.CanEdit.ShouldBe(true);
            result.CanDelete.ShouldBe(true);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new PermissionItemUpdateDto()
            {
                ObjectName = "Demo",
                Description = "3ac79fc3226b44aea9709f2b1de13846423e057d39704ca795d1636ef8aced9b727f85886e6049598eac1ac491170d4843",
                TargetType = "ab1612e9a17648eaae8a3e3e77c50406379ba2ba0e4846eeb214e6326a70b86c6d732c9a9c0d401fb112cfc64b9a11644dc01768499f40d19dc14161a4cfe3e7",
                RoleId = Guid.Empty,
                CanRead = true,
                CanCreate = true,
                CanEdit = true,
                CanDelete = true,
                IsActive = false,
            };

            // Act
            var serviceResult = await _permissionItemsAppService.UpdateAsync(Guid.Parse("b3f400c1-6974-4c36-9275-05d472458284"), input);

            // Assert
            var result = await _permissionItemRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Description.ShouldBe("3ac79fc3226b44aea9709f2b1de13846423e057d39704ca795d1636ef8aced9b727f85886e6049598eac1ac491170d4843");
            result.TargetType.ShouldBe("ab1612e9a17648eaae8a3e3e77c50406379ba2ba0e4846eeb214e6326a70b86c6d732c9a9c0d401fb112cfc64b9a11644dc01768499f40d19dc14161a4cfe3e7");
            result.CanRead.ShouldBe(true);
            result.CanCreate.ShouldBe(true);
            result.CanEdit.ShouldBe(true);
            result.CanDelete.ShouldBe(true);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _permissionItemsAppService.DeleteAsync(Guid.Parse("b3f400c1-6974-4c36-9275-05d472458284"));

            // Assert
            var result = await _permissionItemRepository.FindAsync(c => c.Id == Guid.Parse("b3f400c1-6974-4c36-9275-05d472458284"));

            result.ShouldBeNull();
        }
    }
}