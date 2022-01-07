using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.DeleteAddress;
using DeliveryWebApp.Application.Addresses.Commands.UpdateAddress;
using DeliveryWebApp.Application.Addresses.Queries.GetSingleAddress;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Authorize]
public class AddressesController : ApiControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Address>> Read(int id)
    {
        try
        {
            return await Mediator.Send(new GetSingleAddressQuery
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
    public async Task<ActionResult<Address>> Create([FromBody] Address request)
    {
        return await Mediator.Send(Mapper.Map<CreateAddressCommand>(request));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Address>> Update([FromBody] Address request, int id)
    {
        return await Mediator.Send(new UpdateAddressCommand
        {
            Id = id,
            PlaceName = request.PlaceName,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Address>> Delete(int id)
    {
        return await Mediator.Send(new DeleteAddressCommand
        {
            Id = id
        });
    }
}