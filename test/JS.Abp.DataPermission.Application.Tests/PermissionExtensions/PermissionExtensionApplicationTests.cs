using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public class PermissionExtensionsAppServiceTests : DataPermissionApplicationTestBase
    {
        private readonly IPermissionExtensionsAppService _permissionExtensionsAppService;
        private readonly IRepository<PermissionExtension, Guid> _permissionExtensionRepository;

        public PermissionExtensionsAppServiceTests()
        {
            _permissionExtensionsAppService = GetRequiredService<IPermissionExtensionsAppService>();
            _permissionExtensionRepository = GetRequiredService<IRepository<PermissionExtension, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _permissionExtensionsAppService.GetListAsync(new GetPermissionExtensionsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("064a532a-ae12-44d4-8291-36f3cac0f284")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("1f6c5bc9-9e00-4a21-80b4-d4e7b217f088")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _permissionExtensionsAppService.GetAsync(Guid.Parse("064a532a-ae12-44d4-8291-36f3cac0f284"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("064a532a-ae12-44d4-8291-36f3cac0f284"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new PermissionExtensionCreateDto
            {
                ObjectName = "01babc6fa8ca48e4a9d5bde59dcd2affc669dea214a14bfcac22ffdce7d05039cdb3b4b177cb45209b51fcf1f08738e9d8794ab69f664416b362a0c7113c1695",
                RoleId = Guid.Parse("5d6b19d1-7cda-4a61-96af-a883824fe395"),
                ExcludedRoleId = Guid.Parse("6eff882e-cede-4381-8d65-b028323013a7"),
                PermissionType = default,
                LambdaString = "b8533ff4f5524baf823466193857b7136328d1f04c1",
                IsActive = true
            };

            // Act
            var serviceResult = await _permissionExtensionsAppService.CreateAsync(input);

            // Assert
            var result = await _permissionExtensionRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.ObjectName.ShouldBe("01babc6fa8ca48e4a9d5bde59dcd2affc669dea214a14bfcac22ffdce7d05039cdb3b4b177cb45209b51fcf1f08738e9d8794ab69f664416b362a0c7113c1695");
            result.RoleId.ShouldBe(Guid.Parse("5d6b19d1-7cda-4a61-96af-a883824fe395"));
            result.ExcludedRoleId.ShouldBe(Guid.Parse("6eff882e-cede-4381-8d65-b028323013a7"));
            result.PermissionType.ShouldBe(default);
            result.LambdaString.ShouldBe("b8533ff4f5524baf823466193857b7136328d1f04c1");
            result.IsActive.ShouldBe(true);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new PermissionExtensionUpdateDto()
            {
                ObjectName = "99546e2fb52f4f3e913b62a81c4b616aa02a13ad73894b6cb8ea87576a8c88505f9ba3b8e8c24c359f25f04dbb0e02b4b15cc86b53684e04b95cd95904aa3966",
                RoleId = Guid.Parse("2be9d2ca-c1b7-49a7-aa97-491811b0a38b"),
                ExcludedRoleId = Guid.Parse("e339172c-c283-49b7-832c-502668870d89"),
                PermissionType = default,
                LambdaString = "b3ad1e78c7b74bbb8d03e4a46db8e1f618b4779afb164fedafe3f49f84",
                IsActive = true
            };

            // Act
            var serviceResult = await _permissionExtensionsAppService.UpdateAsync(Guid.Parse("064a532a-ae12-44d4-8291-36f3cac0f284"), input);

            // Assert
            var result = await _permissionExtensionRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.ObjectName.ShouldBe("99546e2fb52f4f3e913b62a81c4b616aa02a13ad73894b6cb8ea87576a8c88505f9ba3b8e8c24c359f25f04dbb0e02b4b15cc86b53684e04b95cd95904aa3966");
            result.RoleId.ShouldBe(Guid.Parse("2be9d2ca-c1b7-49a7-aa97-491811b0a38b"));
            result.ExcludedRoleId.ShouldBe(Guid.Parse("e339172c-c283-49b7-832c-502668870d89"));
            result.PermissionType.ShouldBe(default);
            result.LambdaString.ShouldBe("b3ad1e78c7b74bbb8d03e4a46db8e1f618b4779afb164fedafe3f49f84");
            result.IsActive.ShouldBe(true);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _permissionExtensionsAppService.DeleteAsync(Guid.Parse("064a532a-ae12-44d4-8291-36f3cac0f284"));

            // Assert
            var result = await _permissionExtensionRepository.FindAsync(c => c.Id == Guid.Parse("064a532a-ae12-44d4-8291-36f3cac0f284"));

            result.ShouldBeNull();
        }
    }
}