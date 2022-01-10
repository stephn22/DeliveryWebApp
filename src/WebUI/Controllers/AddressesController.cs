using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.DeleteAddress;
using DeliveryWebApp.Application.Addresses.Commands.UpdateAddress;
using DeliveryWebApp.Application.Addresses.Queries.GetAddressDetail;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Authorize]
public class AddressesController : ApiControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Address>> Read(int id)
    {
        try
        {
            var a = await Mediator.Send(new GetAddressDetailQuery
            {
                Id = id
            });

            return Ok(a);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Address>> Create([FromBody] Address request)
    {
        var a = await Mediator.Send(Mapper.Map<CreateAddressCommand>(request));
        return Ok(a);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Address>> Update([FromBody] Address request, int id)
    {
        try
        {
            var a = await Mediator.Send(new UpdateAddressCommand
            {
                Id = id,
                PlaceName = request.PlaceName,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            });

            return Ok(a);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Address>> Delete(int id)
    {
        try
        {
            var a = await Mediator.Send(new DeleteAddressCommand
            {
                Id = id
            });
            return Ok(a);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}