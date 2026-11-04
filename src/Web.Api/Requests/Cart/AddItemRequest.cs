namespace Web.Api.Requests.Cart
{
    public sealed record AddItemRequest(Guid ProductVariantId, int Quantity);
}