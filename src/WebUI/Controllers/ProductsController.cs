using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Products.Commands.DeleteProduct;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;
using DeliveryWebApp.Application.Products.Queries.GetProducts;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("{id:int}")]
        public async Task<ActionResult<Product>> Create(Product request, int id)
        {
            return await _mediator.Send(new CreateProductCommand
            {
                RestaurateurId = id,
                Name = request.Name,
                Category = request.Category,
                Discount = request.Discount,
                Image = request.Image,
                Price = request.Price,
                Quantity = request.Quantity
            });
        }

        /// <summary>
        /// Get the product list
        /// </summary>
        /// <param name="id">id of the restaurant or the order</param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<List<Product>> Read(int id)
        {
            return await _mediator.Send(new GetProductsQuery
            {
                RestaurateurId = id
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> Update(Product request, int id)
        {
            return await _mediator.Send(_mapper.Map<UpdateProductCommand>(request));
        }

        [HttpDelete]
        public async Task<ActionResult<Product>> Delete(Product request)
        {
            return await _mediator.Send(_mapper.Map<DeleteProductCommand>(request));
        }
    }
}
