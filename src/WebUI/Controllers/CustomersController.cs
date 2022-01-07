using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Customers.Commands.DeleteCustomer;
using DeliveryWebApp.Application.Customers.Commands.UpdateCustomer;
using DeliveryWebApp.Application.Customers.Queries.GetCustomers;
using DeliveryWebApp.Application.Customers.Queries.GetSingleCustomer;
using DeliveryWebApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class CustomersController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ICollection<Customer>>> ReadAll()
        {
            return await Mediator.Send(new GetCustomersQuery());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>> ReadDetails(int id)
        {
            try
            {
                return await Mediator.Send(new GetSingleCustomerQuery
                {
                    Id = id
                });
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Create(Customer request)
        {
            return await Mediator.Send(Mapper.Map<CreateCustomerCommand>(request));
        }

        [HttpPut]
        public async Task<ActionResult<Customer>> Update(Customer request)
        {
            try
            {
                return await Mediator.Send(Mapper.Map<UpdateCustomerCommand>(request));
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        [HttpDelete]
        public async Task<ActionResult<Customer>> Delete(Customer request)
        {
            try
            {
                return await Mediator.Send(Mapper.Map<DeleteCustomerCommand>(request));
            }
            catch (NotFoundException)
            {
                return null;
            };
        }
    }
}
