namespace Web.Api.Requests.Checkout
{
    internal sealed record SubmitShippingRequest(string Email, string FirstName, string LastName, string Address, string City, string ZipCode, string ShippingMethod);
}