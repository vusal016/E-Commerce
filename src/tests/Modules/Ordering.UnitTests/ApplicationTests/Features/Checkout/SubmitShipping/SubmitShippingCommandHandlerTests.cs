
namespace Ordering.UnitTests.ApplicationTests.Features.Checkout.SubmitShipping;

public sealed class SubmitShippingCommandHandlerTests
{
    private readonly IOrderingDbContext _dbContext;
    private readonly SubmitShippingCommandHandler _handler;

    public SubmitShippingCommandHandlerTests()
    {
        _dbContext = Substitute.For<IOrderingDbContext>();
        _handler = new SubmitShippingCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldCreateNewSession_WhenSessionDoesNotExist()
    {
        var command = new SubmitShippingCommand(
            Guid.NewGuid(), 
            "test-session", 
            "test@test.com", 
            "John", 
            "Doe", 
            "123 Main St", 
            "Baku", 
            "AZ1000", 
            "Express"
        );

        var sessions = new List<CheckoutSession>().BuildMockDbSet();
        _dbContext.CheckoutSessions.Returns(sessions);

        var result = await _handler.Handle(command, CancellationToken.None);

        _dbContext.CheckoutSessions.Received(1).Add(Arg.Is<CheckoutSession>(c => 
            c.Email == "test@test.com" && 
            c.ShippingPrice == 15.99m && 
            c.ShippingMethod == "Express"));
            
        await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldUpdateExistingSession_WhenSessionExists()
    {
        var userId = Guid.NewGuid();
        var command = new SubmitShippingCommand(
            userId, 
            "test-session", 
            "updated@test.com", 
            "John", 
            "Doe", 
            "123 Main St", 
            "Baku", 
            "AZ1000", 
            "Same-Day"
        );

        var existingSession = new CheckoutSession(userId, "test-session");
        typeof(CheckoutSession).GetProperty("Id")?.SetValue(existingSession, Guid.NewGuid());
        
        var sessions = new List<CheckoutSession> { existingSession }.BuildMockDbSet();
        _dbContext.CheckoutSessions.Returns(sessions);

        var result = await _handler.Handle(command, CancellationToken.None);

        _dbContext.CheckoutSessions.DidNotReceive().Add(Arg.Any<CheckoutSession>());
        
        existingSession.Email.Should().Be("updated@test.com");
        existingSession.ShippingPrice.Should().Be(25.99m);
        existingSession.ShippingMethod.Should().Be("Same-Day");
            
        await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        
        result.Should().Be(existingSession.Id);
    }
}

