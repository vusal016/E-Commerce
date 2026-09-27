namespace Web.Api.Requests;

public sealed record AddItemRequest(Guid ProductVariantId, int Quantity);
