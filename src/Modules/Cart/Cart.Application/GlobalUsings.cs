global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
global using AutoMapper;
global using Cart.Application.Common.Dtos;
global using Cart.Application.Common.Interfaces;
global using Cart.Application.Features.GetCart;
global using Cart.Application.Features.Queries;
global using Cart.Domain.CartAggregate;
global using Cart.Domain.WishlistAggregate;
global using Catalog.Contracts;
global using Promotions.Contracts;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
global using SharedKernel.Exceptions;

global using Cart.Application.Features.AddItem;
global using Cart.Application.Features.UpdateItemQuantity;
global using Cart.Application.Features.RemoveItem;
global using Cart.Application.Features.ToggleSaveForLater;
global using Cart.Application.Features.ApplyCoupon;
global using Cart.Application.Features.MergeCart;
global using Cart.Application.Features.GetCartSummary;

global using SharedKernel.Events;
global using Ordering.Contracts.Events;
