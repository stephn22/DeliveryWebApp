using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Restaurateurs.Queries.GetRestaurateurs;
using DeliveryWebApp.Application.Restaurateurs.Queries.GetSingleRestaurateur;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurateursController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RestaurateursController> _logger;

        public RestaurateursController(IMediator mediator, ILogger<RestaurateursController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<List<Restaurateur>> Read()
        {
            return await _mediator.Send(new GetRestaurateursQuery());
        }

        [HttpGet("{id:int}")]
        public async Task<Restaurateur> ReadSingle(int id)
        {
            try
            {
                return await _mediator.Send(new GetSingleRestaurateurQuery
                {
                    Id = id
                });
            }
            catch (NotFoundException e)
            {
                _logger.LogWarning($"{e.Message}");
                return null;
            }
        }
    }
}
