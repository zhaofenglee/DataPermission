using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Users;
using Xunit;

namespace JS.Abp.DataPermission.Demos
{
    public abstract class DemosAppServiceTests <TStartupModule> : DataPermissionApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IDemosAppService _demosAppService;
        private readonly IRepository<Demo, Guid> _demoRepository;
        protected ICurrentUser _currentUser;
        public DemosAppServiceTests()
        {
            _demosAppService = GetRequiredService<IDemosAppService>();
            _demoRepository = GetRequiredService<IRepository<Demo, Guid>>();
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
            // Act
            var result = await _demosAppService.GetListAsync(new GetDemosInput());

            // Assert
            result.TotalCount.ShouldBe(3);
            result.Items.Count.ShouldBe(3);
            result.Items.Any(x => x.Id == Guid.Parse("ad9e6084-c77c-40e4-bfee-7f781f8f8a10")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("32603ba2-eb97-487b-8f7a-05d01f1c1a04")).ShouldBe(true);
            
            // simulation of restricted users is performed, which does not allow accessing names "Admin".
            Login(userId: TestData.UserId1); 
            var result2 = await _demosAppService.GetListAsync(new GetDemosInput());
            result2.TotalCount.ShouldBe(2);
            result2.Items.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _demosAppService.GetAsync(Guid.Parse("ad9e6084-c77c-40e4-bfee-7f781f8f8a10"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("ad9e6084-c77c-40e4-bfee-7f781f8f8a10"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new DemoCreateDto
            {
                Name = "acadc4c8e34645eeb18f07e46b56c04b2b7e9a0e47e84b0fbc9f55f95e3bbe6713e78f17757841a1b89aec53291327cbb67648962f5443d8b31031bf79aaa4f9",
                DisplayName = "595fe340d9e940a593be31cfc9d463ffff781556bc5240e099628e93c230821303b9929f8b904287810b628622b8d66fe699909b371a4f20ae1c0043b7e81170"
            };

            // Act
            var serviceResult = await _demosAppService.CreateAsync(input);

            // Assert
            var result = await _demoRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Name.ShouldBe("acadc4c8e34645eeb18f07e46b56c04b2b7e9a0e47e84b0fbc9f55f95e3bbe6713e78f17757841a1b89aec53291327cbb67648962f5443d8b31031bf79aaa4f9");
            result.DisplayName.ShouldBe("595fe340d9e940a593be31cfc9d463ffff781556bc5240e099628e93c230821303b9929f8b904287810b628622b8d66fe699909b371a4f20ae1c0043b7e81170");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new DemoUpdateDto()
            {
                Name = "b052a7e90c3d410dbe1cea5dcf9e8a196f2eeac81dc544b39aa3759f3d5c44a4a00efb42aa1445349623a783b096d6cfa68711de7faf4f48882c6f836494b4a3",
                DisplayName = "27ba89e6df6548268170a953dacea6515b928db5ce784679b49270e4f308ed082eecbb28496b403285e64871f4e638912c17f1a00e734a8a9840d092182df6cc"
            };

            // Act
            var serviceResult = await _demosAppService.UpdateAsync(Guid.Parse("ad9e6084-c77c-40e4-bfee-7f781f8f8a10"), input);

            // Assert
            var result = await _demoRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Name.ShouldBe("b052a7e90c3d410dbe1cea5dcf9e8a196f2eeac81dc544b39aa3759f3d5c44a4a00efb42aa1445349623a783b096d6cfa68711de7faf4f48882c6f836494b4a3");
            result.DisplayName.ShouldBe("27ba89e6df6548268170a953dacea6515b928db5ce784679b49270e4f308ed082eecbb28496b403285e64871f4e638912c17f1a00e734a8a9840d092182df6cc");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _demosAppService.DeleteAsync(Guid.Parse("ad9e6084-c77c-40e4-bfee-7f781f8f8a10"));

            // Assert
            var result = await _demoRepository.FindAsync(c => c.Id == Guid.Parse("ad9e6084-c77c-40e4-bfee-7f781f8f8a10"));

            result.ShouldBeNull();
        }
        
        private void Login(Guid userId)
        {
            _currentUser.Id.Returns(userId);
            _currentUser.IsAuthenticated.Returns(true);
        }
    }
}