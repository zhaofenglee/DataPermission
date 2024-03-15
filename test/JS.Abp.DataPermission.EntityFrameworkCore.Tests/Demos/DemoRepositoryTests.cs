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
                    name: "Admin",
                    displayName: "Test_Admin"
                );
                
                // Assert
                //Cannot view all data without permission.
                result.Count.ShouldBe(0);

                // simulation of restricted users is performed, which does not allow accessing names "Admin".
                Login(userId: TestData.UserId1); 
                // Act
                var result2 = await _demoRepository.GetListAsync(
                    name: "Admin",
                    displayName: "Test_Admin"
                );

                // Assert
                result2.Count.ShouldBe(0);
                
                // Act
                var result3 = await _demoRepository.GetListAsync(
                );

                // Assert
                result3.Count.ShouldBe(2);
                
                //Simulated users without data access permission.
                Login(userId: TestData.UserId2); 
                
                // Act
                var result4 = await _demoRepository.GetListAsync(
                );

                // Assert
                result4.Count.ShouldBe(0);
                
                
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
                );

                // Assert
                result.ShouldBe(0);
                
                Login(userId: TestData.UserId1); 
                // Act
                var result1 = await _demoRepository.GetCountAsync(
                );
                // Assert
                result1.ShouldBe(2);
                
                
            });
        }
        
        private void Login(Guid userId)
        {
            _currentUser.Id.Returns(userId);
            _currentUser.IsAuthenticated.Returns(true);
        }
        
    }
}