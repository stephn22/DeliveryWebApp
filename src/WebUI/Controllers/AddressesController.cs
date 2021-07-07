using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.DeleteAddress;
using DeliveryWebApp.Application.Addresses.Commands.UpdateAddress;
using DeliveryWebApp.Application.Addresses.Queries.GetAddresses;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Newtonsoft.Json.Linq;

namespace DeliveryWebApp.WebUI.Controllers
{
    [Authorize]
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

        [HttpPost]
        public async Task<ActionResult<Address>> Create(Address request)
        {
            return await _mediator.Send(_mapper.Map<CreateAddressCommand>(request));
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
        public async Task<ActionResult<Address>> Update(Address request)
        {
            return await _mediator.Send(_mapper.Map<UpdateAddressCommand>(request));
        }

        [HttpDelete]
        public async Task<ActionResult<Address>> Delete(Address request)
        {
            return await _mediator.Send(_mapper.Map<DeleteAddressCommand>(request));
        }
    }
}
