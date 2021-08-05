using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Orders.Commands.CreateOrder;
using DeliveryWebApp.Application.Orders.Commands.DeleteOrder;
using DeliveryWebApp.Application.Orders.Commands.UpdateOrder;
using DeliveryWebApp.Application.Orders.Queries.GetOrders;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.WebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrdersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create(Order request)
        {
            return await _mediator.Send(_mapper.Map<CreateOrderCommand>(request));
        }

        [HttpGet("{restaurantId:int?}")]
        public async Task<List<Order>> Read(int? restaurantId)
        {
            return await _mediator.Send(new GetOrdersQuery
            {
                RestaurateurId = restaurantId
            });
        }

        [HttpPut]
        public async Task<ActionResult<Order>> Update(Order request)
        {
            return await _mediator.Send(_mapper.Map<UpdateOrderCommand>(request));
        }

        [HttpDelete]
        public async Task<ActionResult<Order>> Delete(Order request)
        {
            return await _mediator.Send(_mapper.Map<DeleteOrderCommand>(request));
        }
    }
}
