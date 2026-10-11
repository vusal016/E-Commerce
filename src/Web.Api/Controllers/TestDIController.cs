using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    internal class TestDIController(IServiceProvider sp) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var services = sp.GetServices<IRequestHandler<Ordering.Application.Features.PlaceOrder.PlaceOrderCommand, Guid>>();
            return Ok(services.Select(x => x.GetType().FullName));
        }
    }
}