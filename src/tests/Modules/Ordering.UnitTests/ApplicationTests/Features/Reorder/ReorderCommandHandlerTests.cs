using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;
using FluentAssertions;
using Ordering.Application.Features.Reorder;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.OrderAggregate;
using Cart.Contracts;
using System.Collections.Generic;

namespace Ordering.UnitTests.ApplicationTests.Features.Reorder;

public sealed class ReorderCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnSuccess_AndNoWarnings_WhenAllItemsAdded()
    {
        var options = new DbContextOptionsBuilder<Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        var dbContext = new Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext(options);
        var userId = Guid.NewGuid();
        
        var order = new Order(userId, "ORD-12345", "Delivered", 100, 10, 5, 0, 115, Guid.NewGuid(), Guid.NewGuid(), "Standard", "TRK001", "FedEx", DateTime.UtcNow.AddDays(-2), null, null);
        var variantId = Guid.NewGuid();
        order.AddItem(variantId, "iPhone 14", 100, 1);
        
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
        
        var mockDbContext = Substitute.For<IOrderingDbContext>();
        mockDbContext.Orders.Returns(dbContext.Orders);
        
        var mockCartApi = Substitute.For<ICartPublicApi>();
        mockCartApi.AddItemAsync(userId, "sess-1", variantId, 1, Arg.Any<CancellationToken>()).Returns(Task.FromResult(true));
        
        var handler = new ReorderCommandHandler(mockDbContext, mockCartApi);
        
        var command = new ReorderCommand(userId, "sess-1", order.Id);
        var result = await handler.Handle(command, CancellationToken.None);
        
        result.Success.Should().BeTrue();
        result.Warnings.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithWarnings_WhenSomeItemsFail()
    {
        var options = new DbContextOptionsBuilder<Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        var dbContext = new Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext(options);
        var userId = Guid.NewGuid();
        
        var order = new Order(userId, "ORD-12345", "Delivered", 100, 10, 5, 0, 115, Guid.NewGuid(), Guid.NewGuid(), "Standard", "TRK001", "FedEx", DateTime.UtcNow.AddDays(-2), null, null);
        var variantId1 = Guid.NewGuid();
        var variantId2 = Guid.NewGuid();
        order.AddItem(variantId1, "iPhone 14", 100, 1);
        order.AddItem(variantId2, "MacBook Pro", 2000, 1);
        
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
        
        var mockDbContext = Substitute.For<IOrderingDbContext>();
        mockDbContext.Orders.Returns(dbContext.Orders);
        
        var mockCartApi = Substitute.For<ICartPublicApi>();
        mockCartApi.AddItemAsync(userId, "sess-1", variantId1, 1, Arg.Any<CancellationToken>()).Returns(Task.FromResult(true));
        mockCartApi.AddItemAsync(userId, "sess-1", variantId2, 1, Arg.Any<CancellationToken>()).Returns(Task.FromResult(false));
        
        var handler = new ReorderCommandHandler(mockDbContext, mockCartApi);
        
        var command = new ReorderCommand(userId, "sess-1", order.Id);
        var result = await handler.Handle(command, CancellationToken.None);
        
        result.Success.Should().BeTrue();
        result.Warnings.Should().HaveCount(1);
        result.Warnings.First().Should().Contain("MacBook Pro");
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
        
        var mockCartApi = Substitute.For<ICartPublicApi>();
        
        var handler = new ReorderCommandHandler(mockDbContext, mockCartApi);
        
        var command = new ReorderCommand(Guid.NewGuid(), "sess-1", Guid.NewGuid());
        
        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}
