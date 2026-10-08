namespace Ordering.Application.Features.GetInvoice;
public sealed record GetInvoiceQuery(Guid? UserId, string OrderNumber) : IRequest<InvoiceDto>;
