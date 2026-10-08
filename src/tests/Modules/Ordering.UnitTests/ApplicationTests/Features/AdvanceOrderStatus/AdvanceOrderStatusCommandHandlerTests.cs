using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;
using FluentAssertions;
using Ordering.Application.Features.AdvanceOrderStatus;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.OrderAggregate;
using System.Collections.Generic;

namespace Ordering.UnitTests.ApplicationTests.Features.AdvanceOrderStatus;

public sealed class AdvanceOrderStatusCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAdvanceStatus_WhenNextStatusExists()
    {
        var options = new DbContextOptionsBuilder<Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        var dbContext = new Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext(options);
        var userId = Guid.NewGuid();
        
        var order = new Order(userId, "ORD-12345", "Pending", 100, 10, 5, 0, 115, Guid.NewGuid(), Guid.NewGuid(), "Standard", "TRK001", "FedEx", DateTime.UtcNow.AddDays(-2), null, null);
        
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
        
        var mockDbContext = Substitute.For<IOrderingDbContext>();
        mockDbContext.Orders.Returns(dbContext.Orders);
        
        var handler = new AdvanceOrderStatusCommandHandler(mockDbContext);
        
        var query = new AdvanceOrderStatusCommand(userId, "ORD-12345");
        var result = await handler.Handle(query, CancellationToken.None);
        
        result.Should().BeTrue();
        order.Status.Should().Be("Processing");
        order.StatusHistory.Should().HaveCount(1);
        order.StatusHistory.First().Status.Should().Be("Processing");
        
        await mockDbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenStatusIsAlreadyDelivered()
    {
        var options = new DbContextOptionsBuilder<Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        var dbContext = new Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext(options);
        var userId = Guid.NewGuid();
        
        var order = new Order(userId, "ORD-12345", "Delivered", 100, 10, 5, 0, 115, Guid.NewGuid(), Guid.NewGuid(), "Standard", "TRK001", "FedEx", DateTime.UtcNow.AddDays(-2), null, null);
        
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
        
        var mockDbContext = Substitute.For<IOrderingDbContext>();
        mockDbContext.Orders.Returns(dbContext.Orders);
        
        var handler = new AdvanceOrderStatusCommandHandler(mockDbContext);
        
        var query = new AdvanceOrderStatusCommand(userId, "ORD-12345");
        var result = await handler.Handle(query, CancellationToken.None);
        
        result.Should().BeFalse();
        order.Status.Should().Be("Delivered");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenOrderNotFound()
    {
        var options = new DbContextOptionsBuilder<Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        var dbContext = new Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext(options);
        var mockDbContext = Substitute.For<IOrderingDbContext>();
        mockDbContext.Orders.Returns(dbContext.Orders);
        
        var handler = new AdvanceOrderStatusCommandHandler(mockDbContext);
        
        var query = new AdvanceOrderStatusCommand(Guid.NewGuid(), "ORD-INVALID");
        
        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));
    }
}
