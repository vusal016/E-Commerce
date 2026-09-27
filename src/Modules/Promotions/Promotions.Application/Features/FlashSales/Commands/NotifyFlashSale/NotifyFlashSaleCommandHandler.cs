namespace Promotions.Application.Features.FlashSales.Commands.NotifyFlashSale
{
    public sealed class NotifyFlashSaleCommandHandler(IPromotionsDbContext dbContext) : IRequestHandler<NotifyFlashSaleCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(NotifyFlashSaleCommand request, CancellationToken cancellationToken)
        {
            var notifyRequest = new NotifyRequest(request.FlashSaleId, request.Email);
            
            await dbContext.NotifyRequests.AddAsync(notifyRequest, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Result<Guid>.Success(notifyRequest.Id, 200);
        }
    }
}
