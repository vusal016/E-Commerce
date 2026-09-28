namespace Ordering.Application.Features.SubmitShipping;

public sealed class SubmitShippingCommandHandler(IOrderingDbContext orderingDb) : IRequestHandler<SubmitShippingCommand, Guid>
{
    public async Task<Guid> Handle(SubmitShippingCommand request, CancellationToken cancellationToken)
    {
        var checkOutSession = await orderingDb.CheckoutSessions.FirstOrDefaultAsync(cs => (request.UserId != null && cs.UserId == request.UserId) || (request.SessionId != null && cs.SessionId == request.SessionId), cancellationToken);

        if (checkOutSession is null)
        {
            checkOutSession = new CheckoutSession(request.UserId, request.SessionId!);
            orderingDb.CheckoutSessions.Add(checkOutSession);
        }

        decimal shippingPrice = request.ShippingMethod switch
        {
            "Express" => 15.99m,
            "Same-Day" => 25.99m,
            _ => 5.99m 
        };

        checkOutSession.SetShippingInfo(request.Email, request.FirstName, request.LastName, request.Address, request.City, request.ZipCode, request.ShippingMethod, shippingPrice);
        
        await orderingDb.SaveChangesAsync(cancellationToken);
        return checkOutSession.Id;
    }
}
