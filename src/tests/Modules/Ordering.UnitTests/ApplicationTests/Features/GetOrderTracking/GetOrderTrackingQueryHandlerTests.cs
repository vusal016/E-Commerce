using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;
using FluentAssertions;
using Ordering.Application.Features.GetOrderTracking;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.OrderAggregate;
using System.Collections.Generic;

namespace Ordering.UnitTests.ApplicationTests.Features.GetOrderTracking;

public sealed class GetOrderTrackingQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnOrderTracking_WhenOrderExists()
    {
        var options = new DbContextOptionsBuilder<Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        var dbContext = new Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext(options);
        var userId = Guid.NewGuid();
        
        var order = new Order(userId, "ORD-TRK123", "Shipped", 100, 10, 5, 0, 115, Guid.NewGuid(), Guid.NewGuid(), "Standard", "TRK001", "FedEx", DateTime.UtcNow.AddDays(-2), null, null);
        
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
        
        var mockDbContext = Substitute.For<IOrderingDbContext>();
        mockDbContext.Orders.Returns(dbContext.Orders);
        
        var handler = new GetOrderTrackingQueryHandler(mockDbContext);
        
        var query = new GetOrderTrackingQuery(userId, "ORD-TRK123");
        var result = await handler.Handle(query, CancellationToken.None);
        
        result.OrderNumber.Should().Be("ORD-TRK123");
        result.CurrentStatus.Should().Be("Shipped");
        result.TrackingNumber.Should().Be("TRK001");
        result.Carrier.Should().Be("FedEx");
        result.History.Should().NotBeNull();
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
        
        var handler = new GetOrderTrackingQueryHandler(mockDbContext);
        
        var query = new GetOrderTrackingQuery(Guid.NewGuid(), "ORD-INVALID");
        
        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));
    }
}
