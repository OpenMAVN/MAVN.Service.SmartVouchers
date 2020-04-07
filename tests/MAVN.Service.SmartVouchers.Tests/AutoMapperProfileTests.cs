using AutoMapper;
using Xunit;

namespace MAVN.Service.SmartVouchers.Tests
{
    public class AutoMapperProfileTests
    {
        [Fact]
        public void Mapping_Configuration_Is_Correct()
        {
            // arrange

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(new Profile[]
                {
                    new AutoMapperProfile(),
                    new MsSqlRepositories.AutoMapperProfile()
                });
            });
            var mapper = mockMapper.CreateMapper();

            // act

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            // assert

            Assert.True(true);
        }
    }
}
