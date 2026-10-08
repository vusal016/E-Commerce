global using Cart.Infrastructure.Persistence.AutoMig;
global using Cart.Infrastructure;
global using Catalog.Application.Common.Dtos;
global using Catalog.Application.Features.CuratedPicks.Queries;
global using Catalog.Application.Features.GetBrandStoreBySlug;
global using Catalog.Application.Features.GetProductById;
global using Catalog.Application.Features.HomeCategories.Queries;
global using Catalog.Application.Features.HomeCategoriesWithFilters;
global using Catalog.Application.Features.SearchProducts;
global using Catalog.Infrastructure.Persistence.AutoMig;
global using Catalog.Infrastructure;
global using Engagement.Infrastructure;
global using Identity.Application.Common.Dtos;
global using Identity.Application.Features.GetRefreshToken;
global using Identity.Application.Features.Login;
global using Identity.Application.Features.Profile.Queries;
global using Identity.Application.Features.Register;
global using Identity.Infrastructure.Persistence.AutoMig;
global using Identity.Infrastructure;
global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Caching.Distributed;
global using Ordering.Infrastructure;
global using Promotions.Application.Common.Dtos;
global using Promotions.Application.Features.FlashSales.Commands.NotifyFlashSale;
global using Promotions.Application.Features.FlashSales.Queries.GetActiveFlashSales;
global using Promotions.Application.Features.FlashSales.Queries.GetUpcomingFlashSales;
global using Promotions.Application.Features.HeroBanners.Queries;
global using Promotions.Infrastructure.Persistence.AutoMig;
global using Promotions.Infrastructure;
global using SharedKernel.Audit;
global using SharedKernel.Cache;
global using SharedKernel.Events;
global using SharedKernel.Exceptions;
global using SharedKernel.Result;
global using System.Security.Claims;
global using System.Text.Json.Serialization;
global using System.Text.Json;
global using System.Threading.Channels;
global using Web.Api.Behaviors;
global using Web.Api.Caching;
global using Web.Api.DatabaseMigrations;
global using Web.Api.Extensions;
global using Web.Api.Interceptor;
global using Web.Api.Messaging.Bus;
global using Web.Api.Messaging.Processing;
global using Web.Api.Messaging.Queue;
global using Web.Api.Messaging;
global using Web.Api.Middleware;
global using Web.Api.Requests;
global using Engagement.Infrastructure.Persistence.AutoMig;
global using Ordering.Infrastructure.Persistence.AutoMig;
global using SharedKernel.CurrentUser;
global using Web.Api.Services;
global using Cart.Application.Features.Queries;
global using Ordering.Application.Features.SubmitShipping;
global using Cart.Application.Common.Dtos;
global using Cart.Application.Features.AddItem;
global using Cart.Application.Features.UpdateItemQuantity;
global using Cart.Application.Features.RemoveItem;
global using Cart.Application.Features.ToggleSaveForLater;
global using Cart.Application.Features.ApplyCoupon;
global using Cart.Application.Features.MergeCart;

global using Web.Api.Requests.Checkout;

global using Ordering.Application.Features.SubmitPayment;

global using Ordering.Application.Features.GetCheckoutSummary;
global using Ordering.Application.Common.Dtos;

global using Ordering.Application.Features.PlaceOrder;


global using Cart.Application.Features.AddWishlistItem;

global using Cart.Application.Features.CreateWishlist;

global using Cart.Application.Features.GetWishlistItems;

global using Cart.Application.Features.GetWishlists;

global using Cart.Application.Features.NotifyWishlistItem;

global using Cart.Application.Features.RemoveWishlistItem;

global using Cart.Application.Features.ShareWishlist;


