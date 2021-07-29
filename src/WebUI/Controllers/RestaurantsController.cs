using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Restaurants.Queries.GetAddress;
using DeliveryWebApp.Application.Restaurants.Queries.GetRestaurants;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.WebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RestaurantsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        //[HttpGet]
        //public ActionResult Details(int id)
        //{
        //    return RedirectToPage("/Customer/RestaurantDetail", routeValues: id);
        //}

        [HttpGet]
        public async Task<List<Restaurant>> Read()
        {
            return await _mediator.Send(new GetRestaurantsQuery());
        }

        [HttpGet("{addressId:int}")]
        public async Task<ActionResult<Address>> GetRestaurantAddress(int addressId)
        {
            return await _mediator.Send(new GetRestaurantAddressQuery
            {
                AddressId = addressId
            });
        }
    }
}
