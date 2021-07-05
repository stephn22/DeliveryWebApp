using AutoMapper;
using DeliveryWebApp.Application.Common.Mappings;
using DeliveryWebApp.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.UpdateAddress;
using DeliveryWebApp.Application.Baskets.Commands.CreateBasket;
using DeliveryWebApp.Application.Baskets.Commands.PurgeBasket;
using DeliveryWebApp.Application.Baskets.Commands.UpdateBasket;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Customers.Commands.DeleteCustomer;
using DeliveryWebApp.Application.Customers.Commands.UpdateCustomer;
using DeliveryWebApp.Application.Orders.Commands.CreateOrder;
using DeliveryWebApp.Application.Orders.Commands.DeleteOrder;
using DeliveryWebApp.Application.Orders.Commands.UpdateOrder;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Products.Commands.DeleteProduct;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;
using DeliveryWebApp.Application.Requests.Commands.CreateRequest;
using DeliveryWebApp.Application.Requests.Commands.DeleteRequest;
using DeliveryWebApp.Application.Requests.Commands.UpdateRequest;
using DeliveryWebApp.Application.Restaurants.Commands.CreateRestaurant;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Application.Restaurateurs.Commands.DeleteRestaurateur;
using DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur;
using DeliveryWebApp.Application.Riders.Commands.CreateRider;
using DeliveryWebApp.Application.Riders.Commands.DeleteRider;
using DeliveryWebApp.Application.Riders.Commands.UpdateRider;
using DeliveryWebApp.Domain.Objects;
using FluentAssertions;
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
                AddressLine1 = "Via Alessandro Manzoni", // new data
                AddressLine2 = "",
                City = "Milan",
                Country = "Italy",
                Number = "21", // new data
                PostalCode = "20090"
            };

            var result = _mapper.Map<UpdateAddressCommand>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<UpdateAddressCommand>();
        }
    }
}