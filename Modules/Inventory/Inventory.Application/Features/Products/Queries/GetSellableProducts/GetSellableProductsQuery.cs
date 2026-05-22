using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Products.Queries.GetSellableProducts;

public record GetSellableProductsQuery(
    string CompanyCen,
    string? Search = null,
    string? CategoryCen = null,
    string? WarehouseCen = null,
    bool OnlyAvailable = true,
    int Page = 1,
    int PageSize = 50
) : IRequest<List<SellableProductContractDto>>;
