using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionItems;
using JS.Abp.DataPermission.EntityFrameworkCore;
using Xunit;

namespace JS.Abp.DataPermission.PermissionItems
{
    public class PermissionItemRepositoryTests : DataPermissionEntityFrameworkCoreTestBase
    {
        private readonly IPermissionItemRepository _permissionItemRepository;

        public PermissionItemRepositoryTests()
        {
            _permissionItemRepository = GetRequiredService<IPermissionItemRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _permissionItemRepository.GetListAsync(
                    objectName: "cf314826ecd44317b980aa74c65f3d6b9444b3af869a4456bfbf2970bdf7eba96d356c758b354e4182719a8fc4ae4570e5c583cf513d4cd0bedb577e6e5b1bfa",
                    description: "906527147df94bd1b3e7e8bd825f4c188b192898b2fe42a58ca06ade19fe54601e0e",
                    targetType: "2db7a10af04e40b4a0d62cd2af9aac61900c5d947c11487ab21d1491ae74aef2dfd13be23df344df88e67023dadd6249632bc4eddf6a4c40b52336da5f24670d",
                    canRead: true,
                    canCreate: true,
                    canEdit: true,
                    canDelete: true
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("b3f400c1-6974-4c36-9275-05d472458284"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _permissionItemRepository.GetCountAsync(
                    objectName: "cb67d9d3da8a49bbb0e159d1d201fab471ea261b78c244dabbd1ca0d7ce0f2418920284a37c64f69bf1a57f07cfdba079c7d87442ca64d13aec3e0a3a5542322",
                    description: "56c9b0e9749949e780a17b681a934",
                    targetType: "072408bac31a488897e962c660178c1540b3c3062bbb41fca483107b2de1993caec2483984b744b2b2d58322b8a4ba4f6e88549322124cc3a6a1fbc090195547",
                    canRead: true,
                    canCreate: true,
                    canEdit: true,
                    canDelete: true
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}