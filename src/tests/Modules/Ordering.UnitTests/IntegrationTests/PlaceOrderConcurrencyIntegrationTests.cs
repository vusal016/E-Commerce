namespace Ordering.UnitTests.IntegrationTests
{
    public sealed class PlaceOrderConcurrencyIntegrationTests(ApiFactory factory) : IClassFixture<ApiFactory>
    {
        [Fact]
        public async Task PlaceOrder_TwoParallelRequests_ForLastItem_OnlyOneSucceeds()
        {
            var variantId = Guid.NewGuid();
            var sessionA = $"session-a-{Guid.NewGuid():N}";
            var sessionB = $"session-b-{Guid.NewGuid():N}";
            using (var scope = factory.Services.CreateScope())
            {
                var catalogDb = scope.ServiceProvider.GetRequiredService<CatalogModuleDbContext>();
                var cartDb = scope.ServiceProvider.GetRequiredService<CartModuleDbContext>();
                var orderingDb = scope.ServiceProvider.GetRequiredService<OrderingModuleDbContext>();
                var brand = new Brand("ConcurrencyBrand", $"brand-{Guid.NewGuid():N}", "https://img/logo.png", "Brand");
                var category = new Category("ConcurrencyCat", $"cat-{Guid.NewGuid():N}", "https://img/icon.png");
                var product = new Product("Limited Product", "Desc", 100m, true, brand.Id, category.Id);
                var variant = new ProductVariant(product.Id, "Black", "L", $"SKU-{Guid.NewGuid():N}", 100m, 100m, 1, true);
                typeof(ProductVariant).BaseType!.GetProperty("Id")!.SetValue(variant, variantId);
                catalogDb.Brands.Add(brand);
                catalogDb.Categories.Add(category);
                catalogDb.Products.Add(product);
                catalogDb.ProductVariants.Add(variant);
                await catalogDb.SaveChangesAsync();
                var cartA = new CartEntity(null, sessionA);
                cartA.Items.Add(new CartItem(cartA.Id, variantId, 1, false));
                var cartB = new CartEntity(null, sessionB);
                cartB.Items.Add(new CartItem(cartB.Id, variantId, 1, false));
                cartDb.Carts.AddRange(cartA, cartB);
                await cartDb.SaveChangesAsync();
                var checkoutA = new CheckoutSession(null, sessionA);
                checkoutA.SetShippingInfo("a@test.com", "A", "User", "Addr", "City", "1000", "Standard", 5m);
                checkoutA.SetPaymentInfo("CreditCard", "1234");
                var checkoutB = new CheckoutSession(null, sessionB);
                checkoutB.SetShippingInfo("b@test.com", "B", "User", "Addr", "City", "1000", "Standard", 5m);
                checkoutB.SetPaymentInfo("CreditCard", "5678");
                orderingDb.CheckoutSessions.AddRange(checkoutA, checkoutB);
                await orderingDb.SaveChangesAsync();
            }
            var clientA = factory.CreateClient();
            clientA.DefaultRequestHeaders.Add("x-session-id", sessionA);
            var clientB = factory.CreateClient();
            clientB.DefaultRequestHeaders.Add("x-session-id", sessionB);
            var responses = await Task.WhenAll(
                clientA.PostAsync("/api/checkout/place-order", null),
                clientB.PostAsync("/api/checkout/place-order", null));
            responses.Count(r => r.StatusCode == HttpStatusCode.OK).Should().Be(1);
            responses.Count(r => r.StatusCode is HttpStatusCode.Conflict or HttpStatusCode.BadRequest).Should().Be(1);
            using (var verifyScope = factory.Services.CreateScope())
            {
                var catalogDb = verifyScope.ServiceProvider.GetRequiredService<CatalogModuleDbContext>();
                var updatedVariant = await catalogDb.ProductVariants.AsNoTracking().FirstAsync(v => v.Id == variantId);
                updatedVariant.StockQuantity.Should().Be(0);
            }
        }
    }
}