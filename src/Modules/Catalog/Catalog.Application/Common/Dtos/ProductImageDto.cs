namespace Catalog.Application.Common.Dtos
{
    public sealed record ProductImageDto(
        Guid Id,
        string ImageUrl,
        bool IsPrimary);
}
