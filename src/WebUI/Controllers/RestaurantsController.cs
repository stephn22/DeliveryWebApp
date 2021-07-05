using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Restaurants.Queries.GetRestaurants;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : Controller
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
    }
}
