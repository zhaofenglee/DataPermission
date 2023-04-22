using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.EntityFrameworkCore;
using Xunit;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public class PermissionExtensionRepositoryTests : DataPermissionEntityFrameworkCoreTestBase
    {
        private readonly IPermissionExtensionRepository _permissionExtensionRepository;

        public PermissionExtensionRepositoryTests()
        {
            _permissionExtensionRepository = GetRequiredService<IPermissionExtensionRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _permissionExtensionRepository.GetListAsync(
                    objectName: "fd5072d8b9664eccb54431f19420e06cf066f43e44234830b3dc10df579d910f382aa3cb0dbe4097aa96d7eb58334064d3d7987e14a847969a0dc8e6b345cfc2",
                    roleId: Guid.Parse("3e5ca2f9-fb65-4819-b4df-e62cae198ae8"),
                    excludedRoleId: Guid.Parse("e7a0e05f-2cca-4d95-a51d-32c433b21a33"),
                    permissionType: default,
                    lambdaString: "52405753f73641bdb43a5261",
                    isActive: true
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("064a532a-ae12-44d4-8291-36f3cac0f284"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _permissionExtensionRepository.GetCountAsync(
                    objectName: "f4f090cd6e794692928a64e8c7b6cc7a94d0bac36fd64e6a95813b8c41c290a40cb2b56f8ab64e7b8f46e3997057a505607b4e1e4fbc495b885c68fb03ac558d",
                    roleId: Guid.Parse("74202aef-0b0d-4ae1-a9cb-85e4cbf8c06e"),
                    excludedRoleId: Guid.Parse("2832d88e-33e9-47ed-9b54-8b8804ca6e12"),
                    permissionType: default,
                    lambdaString: "08676f6",
                    isActive: true
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}