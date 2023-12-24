using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public abstract class ObjectPermissionsAppServiceTests <TStartupModule> : DataPermissionApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IObjectPermissionsAppService _objectPermissionsAppService;
        private readonly IRepository<ObjectPermission, Guid> _objectPermissionRepository;

        public ObjectPermissionsAppServiceTests()
        {
            _objectPermissionsAppService = GetRequiredService<IObjectPermissionsAppService>();
            _objectPermissionRepository = GetRequiredService<IRepository<ObjectPermission, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _objectPermissionsAppService.GetListAsync(new GetObjectPermissionsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("0e96e30d-462c-4b6e-a92c-8a538dafdab2")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("43e88ac8-c5d0-4085-a2b3-8528cf938e74")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _objectPermissionsAppService.GetAsync(Guid.Parse("0e96e30d-462c-4b6e-a92c-8a538dafdab2"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("0e96e30d-462c-4b6e-a92c-8a538dafdab2"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new ObjectPermissionCreateDto
            {
                ObjectName = "e723d559b7dd4a84b241af1efc5e1a8b411d8b3ed8ee43ef8f4b6307e610d8bd7e6afcd01a0e4a47afe196b0307b8365fe2feff0cea742f680740152dce9225b",
                Description = "253b88257589491698b6e948b"
            };

            // Act
            var serviceResult = await _objectPermissionsAppService.CreateAsync(input);

            // Assert
            var result = await _objectPermissionRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.ObjectName.ShouldBe("e723d559b7dd4a84b241af1efc5e1a8b411d8b3ed8ee43ef8f4b6307e610d8bd7e6afcd01a0e4a47afe196b0307b8365fe2feff0cea742f680740152dce9225b");
            result.Description.ShouldBe("253b88257589491698b6e948b");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new ObjectPermissionUpdateDto()
            {
                ObjectName = "Demo",
                Description = "44c316baa55141ae9d60beb7791d3f74cc378944823b46cfaf83f0fd5f308906f66923a3a9504a98bf24e62"
            };

            // Act
            var serviceResult = await _objectPermissionsAppService.UpdateAsync(Guid.Parse("0e96e30d-462c-4b6e-a92c-8a538dafdab2"), input);

            // Assert
            var result = await _objectPermissionRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Description.ShouldBe("44c316baa55141ae9d60beb7791d3f74cc378944823b46cfaf83f0fd5f308906f66923a3a9504a98bf24e62");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _objectPermissionsAppService.DeleteAsync(Guid.Parse("0e96e30d-462c-4b6e-a92c-8a538dafdab2"));

            // Assert
            var result = await _objectPermissionRepository.FindAsync(c => c.Id == Guid.Parse("0e96e30d-462c-4b6e-a92c-8a538dafdab2"));

            result.ShouldBeNull();
        }
    }
}