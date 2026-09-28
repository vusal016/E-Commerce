namespace Ordering.Application.Features.SubmitPayment;

public sealed class SubmitPaymentCommandHandler(IOrderingDbContext orderingDb) : IRequestHandler<SubmitPaymentCommand, Guid>
{
    public async Task<Guid> Handle(SubmitPaymentCommand request, CancellationToken cancellationToken)
    {
        var checkOutSession = await orderingDb.CheckoutSessions.FirstOrDefaultAsync(cs => (request.UserId != null && cs.UserId == request.UserId) || (request.SessionId != null && cs.SessionId == request.SessionId), cancellationToken);

        if (checkOutSession is null)
        {
            checkOutSession = new CheckoutSession(request.UserId, request.SessionId!);
            orderingDb.CheckoutSessions.Add(checkOutSession);
        }

        string? cardLast4 = null;

        if (request.PaymentMethod == "CreditCard")
        {
            if (string.IsNullOrWhiteSpace(request.CardNumber) || request.CardNumber.Length != 16 || !request.CardNumber.All(char.IsDigit))
            {
                throw new ArgumentException("Invalid Card Number.");
            }

            if (string.IsNullOrWhiteSpace(request.Cvv) || (request.Cvv.Length != 3 && request.Cvv.Length != 4) || !request.Cvv.All(char.IsDigit))
            {
                throw new ArgumentException("Invalid CVV.");
            }

            if (string.IsNullOrWhiteSpace(request.ExpiryDate))
            {
                throw new ArgumentException("Invalid Expiry Date.");
            }

            var parts = request.ExpiryDate.Split('/');
            if (parts.Length == 2 && int.TryParse(parts[0], out int month) && int.TryParse(parts[1], out int year))
            {
                var expiry = new DateTime(2000 + year, month, 1).AddMonths(1).AddDays(-1);
                if (expiry < DateTime.UtcNow)
                {
                    throw new ArgumentException("Card is expired.");
                }
            }
            else
            {
                throw new ArgumentException("Invalid expiry format.");
            }

            cardLast4 = request.CardNumber.Substring(request.CardNumber.Length - 4);
        }

        checkOutSession.SetPaymentInfo(request.PaymentMethod, cardLast4);

        await orderingDb.SaveChangesAsync(cancellationToken);
        return checkOutSession.Id;
    }
}
