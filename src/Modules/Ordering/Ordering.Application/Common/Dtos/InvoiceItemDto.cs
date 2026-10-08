namespace Ordering.Application.Common.Dtos;
public sealed record InvoiceItemDto(string ProductName, decimal UnitPrice, int Quantity, decimal TotalPrice);
