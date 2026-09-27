namespace Web.Api.Controllers;

[Route("api/flash-sales")]
[ApiController]
public class FlashSalesController(IMediator mediator) : ControllerBase
{
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveFlashSales(CancellationToken cancellationToken = default)
    {
        var request = new GetActiveFlashSalesQuery();
        var response = await mediator.Send(request, cancellationToken);
        var result = Result<IReadOnlyList<FlashSaleDto>>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingFlashSales(CancellationToken cancellationToken = default)
    {
        var request = new GetUpcomingFlashSalesQuery();
        var response = await mediator.Send(request, cancellationToken);
        var result = Result<IReadOnlyList<FlashSaleDto>>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPost("{id}/notify")]
    public async Task<IActionResult> NotifyFlashSale(Guid id, [FromBody] NotifyFlashSaleRequest request, CancellationToken cancellationToken = default)
    {
        var command = new NotifyFlashSaleCommand(id, request.Email);
        var response = await mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}

