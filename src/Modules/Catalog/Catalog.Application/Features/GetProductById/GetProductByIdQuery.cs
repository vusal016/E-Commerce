namespace Catalog.Application.Features.GetProductById
{
    public sealed record GetProductByIdQuery(Guid Id) : IRequest<GetProductByIdDto>;
}
