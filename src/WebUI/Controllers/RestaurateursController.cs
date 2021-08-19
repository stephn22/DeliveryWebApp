using System;
using AutoMapper;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Restaurateurs.Queries.GetRestaurateurs;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Addresses.Queries.GetSingleAddress;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Restaurateurs.Queries.GetSingleRestaurateur;
using Microsoft.Extensions.Logging;

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

        [HttpGet("/orders/{id:int}")]
        public async Task<Restaurateur> OrdersRead(int id)
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
