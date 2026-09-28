namespace Web.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckoutController(IMediator mediator, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet("summary")]
    public async Task<IActionResult> GetCheckoutSummary(CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new GetCheckoutSummaryQuery(currentUser.UserId, currentUser.SessionId), cancellationToken);
        var result = Result<CheckoutSummaryDto>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPost("shipping")]
    public async Task<IActionResult> SubmitShipping([FromBody] SubmitShippingRequest request, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new SubmitShippingCommand(currentUser.UserId, currentUser.SessionId, request.Email, request.FirstName, request.LastName, request.Address, request.City, request.ZipCode, request.ShippingMethod), cancellationToken);
        var result = Result<Guid>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPost("payment")]
    public async Task<IActionResult> SubmitPayment([FromBody] SubmitPaymentRequest request, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new SubmitPaymentCommand(currentUser.UserId, currentUser.SessionId, request.PaymentMethod, request.CardNumber, request.Cvv, request.ExpiryDate), cancellationToken);
        var result = Result<Guid>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }

    [HttpPost("place-order")]
    public async Task<IActionResult> PlaceOrder(CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new PlaceOrderCommand(currentUser.UserId, currentUser.SessionId), cancellationToken);
        var result = Result<Guid>.Success(response, StatusCodes.Status200OK);
        return Ok(result);
    }
}
