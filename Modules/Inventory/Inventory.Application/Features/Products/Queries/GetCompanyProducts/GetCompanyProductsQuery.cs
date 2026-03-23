using MediatR;
using Inventory.Application.Features.Products.DTOs;

namespace Inventory.Application.Features.Products.Queries.GetCompanyProducts;

public record GetCompanyProductsQuery(Guid CompanyId) : IRequest<List<ProductDto>>;
