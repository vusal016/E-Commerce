namespace Ordering.UnitTests.ApplicationTests.Features.Checkout.SubmitPayment;

public sealed class SubmitPaymentCommandHandlerTests
{
    private readonly IOrderingDbContext _dbContext;
    private readonly SubmitPaymentCommandHandler _handler;

    public SubmitPaymentCommandHandlerTests()
    {
        _dbContext = Substitute.For<IOrderingDbContext>();
        _handler = new SubmitPaymentCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldSaveLast4_WhenPaymentIsCreditCard()
    {
        var command = new SubmitPaymentCommand(Guid.NewGuid(), "session", "CreditCard", "1234567812345678", "123", "12/99");
        var sessions = new List<CheckoutSession>().BuildMockDbSet();
        _dbContext.CheckoutSessions.Returns(sessions);

        var result = await _handler.Handle(command, CancellationToken.None);

        _dbContext.CheckoutSessions.Received(1).Add(Arg.Is<CheckoutSession>(c => c.PaymentMethod == "CreditCard" && c.CardLast4 == "5678"));
        await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldIgnoreCardDetails_WhenPaymentIsPayPal()
    {
        var command = new SubmitPaymentCommand(Guid.NewGuid(), "session", "PayPal", null, null, null);
        var sessions = new List<CheckoutSession>().BuildMockDbSet();
        _dbContext.CheckoutSessions.Returns(sessions);

        var result = await _handler.Handle(command, CancellationToken.None);

        _dbContext.CheckoutSessions.Received(1).Add(Arg.Is<CheckoutSession>(c => c.PaymentMethod == "PayPal" && c.CardLast4 == null));
        await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenCardIsExpired()
    {
        var command = new SubmitPaymentCommand(Guid.NewGuid(), "session", "CreditCard", "1234567812345678", "123", "12/20");
        var sessions = new List<CheckoutSession>().BuildMockDbSet();
        _dbContext.CheckoutSessions.Returns(sessions);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>();
    }
}
