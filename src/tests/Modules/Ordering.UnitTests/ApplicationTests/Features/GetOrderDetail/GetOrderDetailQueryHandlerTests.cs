using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;
using FluentAssertions;
using Ordering.Application.Features.GetOrderDetail;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.OrderAggregate;
using System.Collections.Generic;

namespace Ordering.UnitTests.ApplicationTests.Features.GetOrderDetail;

public sealed class GetOrderDetailQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnOrderDetail_WhenOrderExists()
    {
        var options = new DbContextOptionsBuilder<Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        var dbContext = new Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext(options);
        var userId = Guid.NewGuid();
        
        var order = new Order(userId, "ORD-12345", "Processing", 100, 10, 5, 0, 115, Guid.NewGuid(), Guid.NewGuid(), "Standard", "TRK001", "FedEx", DateTime.UtcNow.AddDays(-2), null, null);
        order.AddItem(Guid.NewGuid(), "iPhone 14", 100, 1);
        
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
        
        var mockDbContext = Substitute.For<IOrderingDbContext>();
        mockDbContext.Orders.Returns(dbContext.Orders);
        
        var handler = new GetOrderDetailQueryHandler(mockDbContext);
        
        var query = new GetOrderDetailQuery(userId, "ORD-12345");
        var result = await handler.Handle(query, CancellationToken.None);
        
        result.OrderNumber.Should().Be("ORD-12345");
        result.Status.Should().Be("Processing");
        result.Total.Should().Be(115);
        result.Items.Should().HaveCount(1);
        result.Items.First().ProductName.Should().Be("iPhone 14");
        result.TrackingNumber.Should().Be("TRK001");
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
        
        var handler = new GetOrderDetailQueryHandler(mockDbContext);
        
        var query = new GetOrderDetailQuery(Guid.NewGuid(), "ORD-INVALID");
        
        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));
    }
}
