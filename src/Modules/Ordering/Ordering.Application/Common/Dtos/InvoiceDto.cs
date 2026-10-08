namespace Ordering.Application.Common.Dtos;
public sealed record InvoiceDto(string InvoiceNumber, string OrderNumber, DateTime IssueDate, Guid? CustomerId, Guid BillingAddressId, IEnumerable<InvoiceItemDto> Items, decimal Subtotal, decimal ShippingCost, decimal Tax, decimal Discount, decimal Total);
