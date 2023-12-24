using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using JS.Abp.DataPermission.Demos;
using JS.Abp.DataPermission.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using Xunit;

namespace JS.Abp.DataPermission.Demos
{
    public class DemoRepositoryTests : DataPermissionEntityFrameworkCoreTestBase
    {
        private readonly IDemoRepository _demoRepository;
        protected ICurrentUser _currentUser;
        public DemoRepositoryTests()
        {
            _demoRepository = GetRequiredService<IDemoRepository>();
        }
        protected override void AfterAddApplication(IServiceCollection services)
        {
            //Mock the current user
            _currentUser = Substitute.For<ICurrentUser>();
            services.AddSingleton(_currentUser);
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                
                // Act
                var result = await _demoRepository.GetListAsync(
                    name: "e3a05b83f7e74467a84068e62dbcc4fd3a1ec607f6b94460bfd1d6086d84ac3dca7dd26aee954f88b9e7cd0054bfeaba0105d7f4e6cd4f75a09b31677324eda9",
                    displayName: "fe8c29ba08de42289ef4622e30bc44d922182760cb6b408c8d35e767744a18d4b6f3107bfda1427cbcaf388c8472bb3bdf15844c9b0343ba9e39e4ada6522e27"
                );
                
                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("ad9e6084-c77c-40e4-bfee-7f781f8f8a10"));
                
                
                // simulation of restricted users is performed, which does not allow accessing names "Admin".
                Login(userId: TestData.UserId1); 
                // Act
                var result2 = await _demoRepository.GetListAsync(
                    name: "Admin",
                    displayName: ""
                );

                // Assert
                result2.Count.ShouldBe(0);
                
                var result3 = await _demoRepository.GetListAsync(
                );

                // Assert
                result3.Count.ShouldBe(2);
                
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _demoRepository.GetCountAsync(
                    name: "2d4f99e927bd4a4a8afedde5dccb2d6eec458f1cba744226b6454f23db3f9c0ae6705bb0154b4c0d8e8b5b2b1e51c9d6275a25370c7045f9bdddf292dec55c63",
                    displayName: "451f7baecf1a49e09ca31800d4b10ff6ef66b05edeb54784ae45fc961a4c1ae4fc80052e6a3d4e92af792fe96986b0bcc22b00a7b66141229f7164b116f015ae"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
        
        private void Login(Guid userId)
        {
            _currentUser.Id.Returns(userId);
            _currentUser.IsAuthenticated.Returns(true);
        }
        
    }
}