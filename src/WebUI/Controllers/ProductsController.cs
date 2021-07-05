using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Products.Queries.GetProducts;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the product list
        /// </summary>
        /// <param name="id">id of the restaurant or the order</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Product>> Read(int id)
        {
            return await _mediator.Send(_mapper.Map<GetProductsQuery>(id));
        }
    }
}
