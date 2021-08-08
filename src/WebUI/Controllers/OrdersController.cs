using AutoMapper;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Orders.Commands.CreateOrder;
using DeliveryWebApp.Application.Orders.Commands.DeleteOrder;
using DeliveryWebApp.Application.Orders.Commands.UpdateOrder;
using DeliveryWebApp.Application.Orders.Queries.GetOrders;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpPost("{id:int}")]
        public async Task<ActionResult<Order>> Create(Order request, int id)
        {
            return await _mediator.Send(_mapper.Map<CreateOrderCommand>(request));
        }

        [HttpGet("{id:int}")]
        public async Task<List<Order>> Read(int id)
        {
            return await _mediator.Send(new GetOrdersQuery
            {
                RestaurateurId = id
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
