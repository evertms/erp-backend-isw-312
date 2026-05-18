using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Products.Queries.GetCompanyProducts;

public record GetCompanyProductsQuery(Guid CompanyId) : IRequest<List<ProductContractDto>>;
