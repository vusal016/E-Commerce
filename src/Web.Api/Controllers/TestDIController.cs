using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestDIController(IServiceProvider sp) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var services = sp.GetServices<IRequestHandler<Ordering.Application.Features.PlaceOrder.PlaceOrderCommand, Guid>>();
        return Ok(services.Select(x => x.GetType().FullName));
    }
}
