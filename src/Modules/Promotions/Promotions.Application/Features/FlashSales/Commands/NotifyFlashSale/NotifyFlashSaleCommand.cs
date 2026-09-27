namespace Promotions.Application.Features.FlashSales.Commands.NotifyFlashSale
{
    public sealed record NotifyFlashSaleCommand(Guid FlashSaleId, string Email) : IRequest<Result<Guid>>;
}
