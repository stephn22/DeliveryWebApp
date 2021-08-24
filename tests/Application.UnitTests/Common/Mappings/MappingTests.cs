using System;
using System.Runtime.Serialization;
using AutoMapper;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.DeleteAddress;
using DeliveryWebApp.Application.Addresses.Commands.UpdateAddress;
using DeliveryWebApp.Application.Common.Mappings;
using DeliveryWebApp.Domain.Entities;
using NUnit.Framework;

namespace DeliveryWebApp.Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }
        // TODO: complete

        [Test]
        [TestCase(typeof(Address), typeof(CreateAddressCommand))]
        [TestCase(typeof(Address), typeof(UpdateAddressCommand))]
        [TestCase(typeof(Address), typeof(DeleteAddressCommand))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);

            _mapper.Map(instance, source, destination);
        }

        private object GetInstanceOf(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
            {
                return Activator.CreateInstance(type);
            }

            // Type without parameterless constructor
            return FormatterServices.GetUninitializedObject(type);
        }
    }
}