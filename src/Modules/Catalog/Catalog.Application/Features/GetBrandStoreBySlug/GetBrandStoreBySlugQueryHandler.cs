namespace Catalog.Application.Features.GetBrandStoreBySlug
{
    public sealed class GetBrandStoreBySlugQueryHandler(ICatalogDbContext catalogDbContext) : IRequestHandler<GetBrandStoreBySlugQuery, BrandStoreDto>
    {
        public async Task<BrandStoreDto> Handle(GetBrandStoreBySlugQuery request, CancellationToken cancellationToken)
        {
            var brand = await catalogDbContext.Brands
                .FirstOrDefaultAsync(b => b.Slug == request.Slug && b.IsActive, cancellationToken);

            if (brand == null)
                throw new KeyNotFoundException("Brand not found.");

            var brandInfo = new BrandInfoDto(brand.Id, brand.Name, brand.Slug, brand.LogoUrl, brand.Description);

            var productsList = await catalogDbContext.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .Include(p => p.ProductTags)
                .Where(p => p.BrandId == brand.Id && p.IsActive)
                .ToListAsync(cancellationToken);

            var bestSellers = productsList
                .Where(p => p.ProductTags.Any(t => t.TagType == Catalog.Domain.Enums.TagType.BestSeller))
                .Take(5)
                .Select(p => new BrandStoreProductDto(
                    p.Id,
                    p.Name,
                    p.BasePrice,
                    p.ProductImages.FirstOrDefault(i => i.IsPrimary)?.ImageUrl))
                .ToList();

            var products = productsList
                .Select(p => new BrandStoreProductDto(
                    p.Id,
                    p.Name,
                    p.BasePrice,
                    p.ProductImages.FirstOrDefault(i => i.IsPrimary)?.ImageUrl))
                .ToList();

            var minPrice = productsList.Count > 0 ? productsList.Min(p => p.BasePrice) : 0;
            var maxPrice = productsList.Count > 0 ? productsList.Max(p => p.BasePrice) : 0;
            var colors = productsList.SelectMany(p => p.ProductVariants).Select(v => v.Color).Distinct().ToList();
            var sizes = productsList.SelectMany(p => p.ProductVariants).Select(v => v.Size).Distinct().ToList();

            var filters = new BrandStoreFiltersDto(minPrice, maxPrice, colors, sizes);

            return new BrandStoreDto(brandInfo, bestSellers, products, filters);
        }
    }
}
