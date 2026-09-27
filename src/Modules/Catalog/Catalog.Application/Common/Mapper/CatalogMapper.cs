namespace Catalog.Application.Common.Mapper
{
    public sealed class CatalogMapper : Profile
    {
        public CatalogMapper()
        {
            CreateMap<Product, CuratedPickDto>();
            CreateMap<Category, HomeCategoryDto>();
            
            CreateMap<Brand, BrandInfoDto>();
            CreateMap<Product, BrandStoreProductDto>()
                .ForCtorParam("PrimaryImageUrl", opt => opt.MapFrom(src => 
                    src.ProductImages.FirstOrDefault(i => i.IsPrimary) != null 
                        ? src.ProductImages.FirstOrDefault(i => i.IsPrimary)!.ImageUrl 
                        : null));
        }
    }
}
