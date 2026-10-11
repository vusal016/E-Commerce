namespace Web.Api.Requests.Cart
{
    internal sealed record AddItemRequest(Guid ProductVariantId, int Quantity);
}