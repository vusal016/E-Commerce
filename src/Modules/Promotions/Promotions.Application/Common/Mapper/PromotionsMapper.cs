namespace Promotions.Application.Common.Mapper
{
    public sealed class PromotionsMapper : Profile
    {
        public PromotionsMapper()
        {
            CreateMap<HeroBanner, HeroBannerDto>();
        }
    }
}

