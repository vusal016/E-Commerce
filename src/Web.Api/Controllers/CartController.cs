namespace Web.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController(IMediator mediator, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCart(CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new GetCartQuery(currentUser.UserId, currentUser.SessionId), cancellationToken);
        var result = Result<CartDto>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetCartSummary(CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new GetCartSummaryQuery(currentUser.UserId, currentUser.SessionId), cancellationToken);
        var result = Result<CartSummaryDto>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddItemRequest request, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new AddItemCommand(currentUser.UserId, currentUser.SessionId, request.ProductVariantId, request.Quantity), cancellationToken);
        var result = Result<CartDto>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPatch("items/{id}")]
    public async Task<IActionResult> UpdateItemQuantity(Guid id, [FromBody] UpdateQuantityRequest request, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new UpdateItemQuantityCommand(currentUser.UserId, currentUser.SessionId, id, request.Quantity), cancellationToken);
        var result = Result<CartDto>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpDelete("items/{id}")]
    public async Task<IActionResult> RemoveItem(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new RemoveItemCommand(currentUser.UserId, currentUser.SessionId, id), cancellationToken);
        var result = Result<CartDto>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPatch("items/{id}/save-for-later")]
    public async Task<IActionResult> ToggleSaveForLater(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new ToggleSaveForLaterCommand(currentUser.UserId, currentUser.SessionId, id), cancellationToken);
        var result = Result<CartDto>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPost("apply-coupon")]
    public async Task<IActionResult> ApplyCoupon([FromBody] ApplyCouponRequest request, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new ApplyCouponCommand(currentUser.UserId, currentUser.SessionId, request.Code), cancellationToken);
        var result = Result<CartDto>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPost("merge")]
    [Authorize]
    public async Task<IActionResult> MergeCart(CancellationToken cancellationToken = default)
    {
        if (currentUser.UserId == null || string.IsNullOrWhiteSpace(currentUser.SessionId))
            return BadRequest(Result<CartDto>.Fail("User must be logged in and have a session ID to merge carts.", StatusCodes.Status400BadRequest));
            
        var response = await mediator.Send(new MergeCartCommand(currentUser.UserId.Value, currentUser.SessionId), cancellationToken);
        var result = Result<CartDto>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }
}
