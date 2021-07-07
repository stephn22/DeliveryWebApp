using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Baskets.Commands.UpdateBasket;
using DeliveryWebApp.Application.Baskets.Queries;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Domain.Objects;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        [HttpGet]
        public async Task<Basket> Read(int customerId)
        {
            return await _mediator.Send(new GetBasketQuery
            {
                CustomerId = customerId
            });
        }

        [HttpPut]
        public async Task<ActionResult<Basket>> Update(JObject data)
        {
            dynamic jsonData = data;

            //JObject customerIdJson = jsonData.customerId;
            JObject productJson = jsonData.product;

            var product = productJson.ToObject<Product>();

            var request = new AddToBasket
            {
                CustomerId = jsonData.customerId,
                Product = product
            };

            return await _mediator.Send(_mapper.Map<UpdateBasketCommand>(request));
        }
    }
}
