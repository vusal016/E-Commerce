namespace Catalog.Application.Common.Dtos
{
    public sealed record HomeCategoryDto(
        Guid Id,
        string Name,
        string Slug,
        string IconUrl);
}