using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Baskets.Commands.UpdateBasket;
using DeliveryWebApp.Application.Baskets.Queries;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BasketsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Basket> Read(int customerId)
        {
            return await _mediator.Send(new GetBasketQuery
            {
                CustomerId = customerId
            });
        }

        [HttpPut]
        public async Task<ActionResult<Basket>> Update(Basket request)
        {
            return await _mediator.Send(_mapper.Map<UpdateBasketCommand>(request));
        }
    }
}
