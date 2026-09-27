global using AutoMapper;
global using Catalog.Application.Common.Dtos;
global using Catalog.Application.Common.Interfaces;
global using Catalog.Application.Common.Mapper;
global using Catalog.Application.Features.GetBrandStoreBySlug;
global using Catalog.Application.Features.GetProductById;
global using Catalog.Application.Features.SearchProducts;
global using Catalog.Domain.Brand;
global using Catalog.Domain.ProductAggregate;
global using FluentAssertions;
global using Microsoft.EntityFrameworkCore;
global using MockQueryable.NSubstitute;
global using NSubstitute;
global using Xunit;

global using Engagement.Contracts;
global using Promotions.Contracts;
