using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using JS.Abp.DataPermission.ObjectPermissions;
using JS.Abp.DataPermission.EntityFrameworkCore;
using Xunit;

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public class ObjectPermissionRepositoryTests : DataPermissionEntityFrameworkCoreTestBase
    {
        private readonly IObjectPermissionRepository _objectPermissionRepository;

        public ObjectPermissionRepositoryTests()
        {
            _objectPermissionRepository = GetRequiredService<IObjectPermissionRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _objectPermissionRepository.GetListAsync(
                    objectName: "53a3ee1a28ff443984bf9a2e95a9f05c7c1c8a77e1ff42a1864d48afac7f3acb716ffd81c76148ecb03a3223bf53dc8be986f48096c94fc18a07b32e64f96398",
                    description: "8d195358b663424e819670724d04021100feb2a79b424cf9a"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("0e96e30d-462c-4b6e-a92c-8a538dafdab2"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _objectPermissionRepository.GetCountAsync(
                    objectName: "3f1ac3871b2b4e9d9a14a21217b51f65577673c5aa914620bb96f3a580cc2b24e3d6d23ae3144a309c287623793e0e117a3ee07d24f34f9bbde95403f9a90f48",
                    description: "9099bdfdc7084050b42d611d714007c8e484fd6d502446"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}