using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Customers.Commands.DeleteCustomer;
using DeliveryWebApp.Application.Customers.Commands.UpdateCustomer;
using DeliveryWebApp.Application.Customers.Queries.GetCustomerDetail;
using DeliveryWebApp.Application.Customers.Queries.GetCustomers;
using DeliveryWebApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class CustomersController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ICollection<Customer>>> ReadAll()
        {
            var customers = await Mediator.Send(new GetCustomersQuery());
            return Ok(customers);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Customer>> ReadDetails(int id)
        {
            try
            {
                var customer = await Mediator.Send(new GetCustomerDetailQuery
                {
                    Id = id
                });

                return Ok(customer);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Customer>> Create(Customer request)
        {
            var c = await Mediator.Send(Mapper.Map<CreateCustomerCommand>(request));
            return Ok(c);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Customer>> Update(Customer request)
        {
            try
            {
                var c = await Mediator.Send(Mapper.Map<UpdateCustomerCommand>(request));
                return Ok(c);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Customer>> Delete(Customer request)
        {
            try
            {
                var c = await Mediator.Send(Mapper.Map<DeleteCustomerCommand>(request));
                return Ok(c);
            }
            catch (NotFoundException)
            {
                return NotFound();
            };
        }
    }
}
