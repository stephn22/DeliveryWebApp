﻿using AutoMapper;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Customers.Commands.DeleteCustomer;
using DeliveryWebApp.Application.Customers.Commands.UpdateCustomer;
using DeliveryWebApp.Application.Customers.Queries.GetCustomers;
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
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CustomersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        // GET: CustomersController/Details/5
        [HttpGet]
        public Microsoft.AspNetCore.Mvc.ActionResult Details(int id)
        {
            return RedirectToPage("/AdminPages/CustomerDetail", routeValues: id);
        }

        // GET: CustomersController/Edit/5
        [HttpGet]
        public Microsoft.AspNetCore.Mvc.ActionResult Edit(int id)
        {
            return RedirectToPage("/AdminPages/CustomerDetail", routeValues: id);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Create(Customer request)
        {
            return await _mediator.Send(_mapper.Map<CreateCustomerCommand>(request));
        }

        [HttpGet]
        public async Task<List<Customer>> Read()
        {
            return await _mediator.Send(new GetCustomersQuery());
        }

        [HttpPut]
        public async Task<ActionResult<Customer>> Update(Customer request)
        {
            return await _mediator.Send(_mapper.Map<UpdateCustomerCommand>(request));
        }

        [HttpDelete]
        public async Task<ActionResult<Customer>> Delete(Customer request)
        {
            return await _mediator.Send(_mapper.Map<DeleteCustomerCommand>(request));
        }
    }
}
