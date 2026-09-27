namespace Catalog.Application.Common.Dtos;

public sealed record GetProductsWithFiltersDto(
    Guid Id,
    string Name,
    decimal BasePrice,
    string CategoryName,
    string? PrimaryImageUrl,
    List<VariantSimpleDto> Variants);

public sealed record VariantSimpleDto(
    Guid Id,
    string Color,
    string Size,
    decimal Price,
    decimal OriginalPrice,
    bool InStock);
