﻿using AutoMapper;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.DeleteAddress;
using DeliveryWebApp.Application.Addresses.Commands.UpdateAddress;
using DeliveryWebApp.Application.Addresses.Queries.GetSingleAddress;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Controllers
{
    [Authorize(Policy = PolicyName.IsAuthenticated)]
    [Authorize(Policy = PolicyName.IsCustomer)]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressesController> _logger;

        public AddressesController(IMediator mediator, IMapper mapper, ILogger<AddressesController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Address>> Create([FromBody] Address request)
        {
            return await _mediator.Send(_mapper.Map<CreateAddressCommand>(request));
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Address>> Read(int id)
        {
            try
            {
                return await _mediator.Send(new GetSingleAddressQuery
                {
                    Id = id
                });
            }
            catch (NotFoundException e)
            {
                _logger.LogWarning($"{e.Message}");
                return null;
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Address>> Update([FromBody] Address request, int id)
        {
            return await _mediator.Send(new UpdateAddressCommand
            {
                Id = id,
                PlaceName = request.PlaceName,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Address>> Delete(int id)
        {
            return await _mediator.Send(new DeleteAddressCommand
            {
                Id = id
            });
        }
    }
}
