using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryWebApp.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BasketItemsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

    }
}
