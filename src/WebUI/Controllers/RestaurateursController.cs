using AutoMapper;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Restaurateurs.Queries.GetRestaurateurAddress;
using DeliveryWebApp.Application.Restaurateurs.Queries.GetRestaurateurs;
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
    public class RestaurateursController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RestaurateursController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<Restaurateur>> Read()
        {
            return await _mediator.Send(new GetRestaurateursQuery());
        }
    }
}
