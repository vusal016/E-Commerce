namespace Web.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WishlistsController(IMediator mediator, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetWishlists(CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new GetWishlistsQuery(currentUser.UserId), cancellationToken);
        var result = Result<IEnumerable<WishlistDto>>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWishlist([FromBody] CreateWishlistRequest request, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new CreateWishlistCommand(currentUser.UserId, request.Name), cancellationToken);
        var result = Result<Guid>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetWishlistItems(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new GetWishlistItemsQuery(currentUser.UserId, id), cancellationToken);
        var result = Result<IEnumerable<WishlistItemDetailDto>>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPost("{id}/items")]
    public async Task<IActionResult> AddWishlistItem(Guid id, [FromBody] AddWishlistItemRequest request, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new AddWishlistItemCommand(currentUser.UserId, id, request.ProductVariantId), cancellationToken);
        var result = Result<Guid>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpDelete("{id}/items/{itemId}")]
    public async Task<IActionResult> RemoveWishlistItem(Guid id, Guid itemId, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new RemoveWishlistItemCommand(currentUser.UserId, id, itemId), cancellationToken);
        var result = Result<bool>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPost("{id}/items/{itemId}/notify")]
    public async Task<IActionResult> NotifyWishlistItem(Guid id, Guid itemId, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new NotifyWishlistItemCommand(currentUser.UserId, id, itemId), cancellationToken);
        var result = Result<bool>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpGet("{id}/share")]
    public async Task<IActionResult> ShareWishlist(Guid id, CancellationToken cancellationToken = default)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var response = await mediator.Send(new ShareWishlistCommand(currentUser.UserId, id, baseUrl), cancellationToken);
        var result = Result<string>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }
}

