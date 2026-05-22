using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Products.Queries.GetCompanyProducts;

public record GetCompanyProductsQuery(
    string CompanyCen, 
    string? Search = null, 
    string? CategoryCen = null, 
    string? Status = null) : IRequest<List<ProductContractDto>>;
