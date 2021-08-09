using AutoMapper;
using DeliveryWebApp.Application.Baskets.Commands.UpdateBasket;
using DeliveryWebApp.Application.Baskets.Queries;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Controllers
{
    [Authorize]
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

        //[HttpGet] TODO:
        //public async Task<Basket> Read(int customerId)
        //{
        //    return await _mediator.Send(new GetBasketQuery
        //    {
        //        Customer = customerId
        //    });
        //}

        [HttpPut("{customerId:int}")]
        public async Task<ActionResult<Basket>> Update(int customerId, Product request)
        {
            return await _mediator.Send(new UpdateBasketCommand
            {
                CustomerId = customerId,
                Product = request
            });
        }
    }
}
