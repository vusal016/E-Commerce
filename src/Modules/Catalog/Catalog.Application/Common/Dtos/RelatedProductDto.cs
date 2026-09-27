namespace Catalog.Application.Common.Dtos
{
    public sealed record RelatedProductDto(
        Guid Id,
        string Name,
        decimal BasePrice,
        string? PrimaryImageUrl);
}
