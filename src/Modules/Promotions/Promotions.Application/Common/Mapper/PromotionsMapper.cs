namespace Promotions.Application.Common.Mapper
{
    public sealed class PromotionsMapper : Profile
    {
        public PromotionsMapper()
        {
            CreateMap<HeroBanner, HeroBannerDto>();

            CreateMap<FlashSale, FlashSaleDto>();

            CreateMap<FlashSaleItem, FlashSaleItemDto>()
                .ForMember(d => d.SoldPercentage, opt => opt.MapFrom(s => 
                    s.StockLimit > 0 ? Math.Round((double)s.SoldCount / s.StockLimit * 100, 2) : 0));
        }
    }
}
