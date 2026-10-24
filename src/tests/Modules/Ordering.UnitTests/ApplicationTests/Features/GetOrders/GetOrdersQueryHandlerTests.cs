namespace Ordering.UnitTests.ApplicationTests.Features.GetOrders
{
    internal class TestOrderingDbContext : DbContext, IOrderingDbContext
    {
        public TestOrderingDbContext(DbContextOptions<TestOrderingDbContext> options) : base(options) { }
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; } = null!;
        public DbSet<ReturnRequest> ReturnRequests { get; set; } = null!;
        public DbSet<CheckoutSession> CheckoutSessions { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<OrderItem>().HasKey(i => i.Id);
        }
    }
    public sealed class GetOrdersQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnPaginatedOrders_FilteredByUserIdAndStatusAndSearch()
        {
            var options = new DbContextOptionsBuilder<TestOrderingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var dbContext = new TestOrderingDbContext(options);
            var userId = Guid.NewGuid();
            var order1 = new Order(userId, "ORD-001", "Processing", 100, 10, 5, 0, 115, Guid.NewGuid(), Guid.NewGuid(), "Standard", null, null, DateTime.UtcNow.AddDays(-2), null, null);
            order1.AddItem(Guid.NewGuid(), "iPhone 14", 100, 1);
            var order2 = new Order(userId, "ORD-002", "Shipped", 200, 10, 10, 0, 220, Guid.NewGuid(), Guid.NewGuid(), "Express", "TRK123", "FedEx", DateTime.UtcNow.AddDays(-1), null, null);
            order2.AddItem(Guid.NewGuid(), "MacBook Pro", 200, 1);
            var order3 = new Order(Guid.NewGuid(), "ORD-003", "Processing", 50, 5, 2, 0, 57, Guid.NewGuid(), Guid.NewGuid(), "Standard", null, null, DateTime.UtcNow, null, null);
            order3.AddItem(Guid.NewGuid(), "AirPods", 50, 1);
            dbContext.Orders.AddRange(order1, order2, order3);
            await dbContext.SaveChangesAsync();
            var mockDbContext = Substitute.For<IOrderingDbContext>();
            mockDbContext.Orders.Returns(dbContext.Orders);
            var handler = new GetOrdersQueryHandler(mockDbContext);
            var query1 = new GetOrdersQuery(userId, null, null, 1);
            var result1 = await handler.Handle(query1, CancellationToken.None);
            result1.Data.Should().HaveCount(2);
            result1.Pagination.TotalCount.Should().Be(2);
            var query2 = new GetOrdersQuery(userId, "processing", null, 1);
            var result2 = await handler.Handle(query2, CancellationToken.None);
            result2.Data.Should().HaveCount(1);
            result2.Data.First().OrderNumber.Should().Be("ORD-001");
            var query3 = new GetOrdersQuery(userId, null, "macbook", 1);
            var result3 = await handler.Handle(query3, CancellationToken.None);
            result3.Data.Should().HaveCount(1);
            result3.Data.First().OrderNumber.Should().Be("ORD-002");
            var query4 = new GetOrdersQuery(userId, null, null, 2);
            var result4 = await handler.Handle(query4, CancellationToken.None);
            result4.Data.Should().BeEmpty();
            result4.Pagination.TotalPages.Should().Be(1);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        public async Task Handle_WhenPageIsZeroOrNegative_ShouldNotThrowAndShouldClampCurrentPageToOne(int invalidPage)
        {
            DbContextOptions<TestOrderingDbContext> options = new DbContextOptionsBuilder<TestOrderingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using TestOrderingDbContext dbContext = new TestOrderingDbContext(options);
            Guid userId = Guid.NewGuid();
            Order order = new Order(userId, "ORD-001", "Processing", 100, 10, 5, 0, 115, Guid.NewGuid(), Guid.NewGuid(), "Standard", null, null, DateTime.UtcNow, null, null);
            order.AddItem(Guid.NewGuid(), "iPhone 14", 100, 1);
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            IOrderingDbContext mockDbContext = Substitute.For<IOrderingDbContext>();
            mockDbContext.Orders.Returns(dbContext.Orders);
            GetOrdersQueryHandler handler = new GetOrdersQueryHandler(mockDbContext);
            GetOrdersQuery query = new GetOrdersQuery(userId, null, null, invalidPage);
            Func<Task<(IEnumerable<OrderDto> Data, PaginationInfo Pagination)>> act = async () => await handler.Handle(query, CancellationToken.None);
            await act.Should().NotThrowAsync();
            (IEnumerable<OrderDto> Data, PaginationInfo Pagination) result = await handler.Handle(query, CancellationToken.None);
            result.Pagination.CurrentPage.Should().Be(1);
            result.Data.Should().HaveCount(1);
        }
    }
}