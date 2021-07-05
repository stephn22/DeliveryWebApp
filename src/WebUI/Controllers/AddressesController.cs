using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Queries.GetAddresses;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Newtonsoft.Json.Linq;

namespace DeliveryWebApp.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AddressesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<Address>> Read(int customerId)
        {
            return await _mediator.Send(new GetAddressesQuery
            {
                CustomerId = customerId
            });
        }

        [HttpPut]
        public async Task<ActionResult<Address>> Update(JObject data)
        {
            dynamic json = data;
            var address = new Address
            {
                AddressLine1 = json.addressLine1,
                AddressLine2 = json.addressLine2,
                Number = json.number,
                PostalCode = json.postalCode,
                City = json.city,
                StateProvince = json.stateProvince,
                Country = json.country,
                CustomerId = json.customerId
            };

            return await _mediator.Send(_mapper.Map<CreateAddressCommand>(address));
        }

        [HttpDelete]
        public async Task<ActionResult<Address>> Delete(Address request)
        {
            //TODO: implement DeleteAddressCommand
            return null;
        }
    }
}
