using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;
using FluentAssertions;
using Ordering.Application.Features.GetInvoice;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.OrderAggregate;
using System.Collections.Generic;

namespace Ordering.UnitTests.ApplicationTests.Features.GetInvoice;

public sealed class GetInvoiceQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnInvoice_WhenOrderExists()
    {
        var options = new DbContextOptionsBuilder<Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        var dbContext = new Ordering.UnitTests.ApplicationTests.Features.GetOrders.TestOrderingDbContext(options);
        var userId = Guid.NewGuid();
        
        var order = new Order(userId, "ORD-777", "Delivered", 100, 10, 5, 0, 115, Guid.NewGuid(), Guid.NewGuid(), "Standard", "TRK001", "FedEx", DateTime.UtcNow.AddDays(-2), null, null);
        order.AddItem(Guid.NewGuid(), "iPhone 14", 100, 2);
        
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
        
        var mockDbContext = Substitute.For<IOrderingDbContext>();
        mockDbContext.Orders.Returns(dbContext.Orders);
        
        var handler = new GetInvoiceQueryHandler(mockDbContext);
        
        var query = new GetInvoiceQuery(userId, "ORD-777");
        var result = await handler.Handle(query, CancellationToken.None);
        
        result.OrderNumber.Should().Be("ORD-777");
        result.InvoiceNumber.Should().Be("INV-ORD-777");
        result.Subtotal.Should().Be(100);
        result.Total.Should().Be(115);
        result.Items.Should().HaveCount(1);
        result.Items.First().ProductName.Should().Be("iPhone 14");
        result.Items.First().TotalPrice.Should().Be(200);
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
        
        var handler = new GetInvoiceQueryHandler(mockDbContext);
        
        var query = new GetInvoiceQuery(Guid.NewGuid(), "ORD-INVALID");
        
        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));
    }
}
