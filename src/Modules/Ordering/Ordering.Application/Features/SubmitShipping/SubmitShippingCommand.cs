namespace Ordering.Application.Features.SubmitShipping;

public sealed record SubmitShippingCommand(Guid? UserId, string? SessionId, string Email, string FirstName, string LastName, string Address, string City, string ZipCode, string ShippingMethod) : IRequest<Guid>;
