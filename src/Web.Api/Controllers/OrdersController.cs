using Ordering.Application.Features.GetOrders;
using Ordering.Application.Features.GetOrderDetail;
using Ordering.Application.Features.GetOrderTracking;
using Ordering.Application.Features.AdvanceOrderStatus;
using Ordering.Application.Features.Reorder;
using Ordering.Application.Features.GetInvoice;
namespace Web.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IMediator mediator, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] string? status, [FromQuery] string? search, [FromQuery] int page = 1, CancellationToken cancellationToken = default)
    {
        var (data, pagination) = await mediator.Send(new GetOrdersQuery(currentUser.UserId, status, search, page), cancellationToken);
        var result = Result<IEnumerable<OrderDto>>.Success(data, StatusCodes.Status200OK);
        result.SetPaginationInfo(pagination);
        return Ok(result);
    }

    [HttpGet("{orderNumber}")]
    public async Task<IActionResult> GetOrderDetail(string orderNumber, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new GetOrderDetailQuery(currentUser.UserId, orderNumber), cancellationToken);
        var result = Result<OrderDetailDto>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpGet("{orderNumber}/tracking")]
    public async Task<IActionResult> GetOrderTracking(string orderNumber, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new GetOrderTrackingQuery(currentUser.UserId, orderNumber), cancellationToken);
        var result = Result<OrderTrackingDto>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPatch("{orderNumber}/advance-status")]
    [Tags("Test/Dev only")]
    public async Task<IActionResult> AdvanceOrderStatus(string orderNumber, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new AdvanceOrderStatusCommand(currentUser.UserId, orderNumber), cancellationToken);
        var result = Result<bool>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPost("{orderId}/reorder")]
    public async Task<IActionResult> Reorder(Guid orderId, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new ReorderCommand(currentUser.UserId, currentUser.SessionId, orderId), cancellationToken);
        var result = Result<ReorderResultDto>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpGet("{orderNumber}/invoice")]
    public async Task<IActionResult> GetInvoice(string orderNumber, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new GetInvoiceQuery(currentUser.UserId, orderNumber), cancellationToken);
        var result = Result<InvoiceDto>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }
}


