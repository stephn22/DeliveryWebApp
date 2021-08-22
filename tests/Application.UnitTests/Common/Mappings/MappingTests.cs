using AutoMapper;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.UpdateAddress;
using Shouldly;
using Xunit;

namespace DeliveryWebApp.Application.UnitTests.Common.Mappings
{
    public class MappingTests : IClassFixture<MappingTestsFixture>
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests(MappingTestsFixture fixture)
        {
            _configuration = fixture.ConfigurationProvider;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void ShouldMapAddressToCreateAddressCommand()
        {
            var entity = new CreateAddressCommand
            {
                AddressLine1 = "Via Verdi",
                AddressLine2 = "",
                City = "Milan",
                Country = "Italy",
                Number = "12",
                PostalCode = "20090"
            };

            var result = _mapper.Map<CreateAddressCommand>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<CreateAddressCommand>();
        }

        [Fact]
        public void ShouldMapAddressToUpdateAddressCommand()
        {
            var entity = new UpdateAddressCommand
            {
                // TODO:
            };

            var result = _mapper.Map<UpdateAddressCommand>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<UpdateAddressCommand>();
        }
    }
}