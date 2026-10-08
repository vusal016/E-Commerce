namespace Catalog.Application.Common.Dtos
{
    public sealed record BrandInfoDto(
        Guid Id,
        string Name,
        string Slug,
        string LogoUrl,
        string Description);
}