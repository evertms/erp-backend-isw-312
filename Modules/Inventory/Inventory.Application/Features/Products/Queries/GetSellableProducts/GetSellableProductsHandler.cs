using MediatR;
using Inventory.Domain.Repositories;
using Shared.Contracts.Inventory;
using Inventory.Domain.Enums;

namespace Inventory.Application.Features.Products.Queries.GetSellableProducts;

public class GetSellableProductsHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IProductStockRepository stockRepository,
    IWarehouseRepository warehouseRepository) : IRequestHandler<GetSellableProductsQuery, List<SellableProductContractDto>>
{
    public async Task<List<SellableProductContractDto>> Handle(GetSellableProductsQuery request, CancellationToken cancellationToken)
    {
        int? categoryId = null;
        if (!string.IsNullOrEmpty(request.CategoryCen))
        {
            var category = await categoryRepository.GetByCenAsync(request.CategoryCen, cancellationToken);
            categoryId = category?.Id;
        }

        int? warehouseId = null;
        if (!string.IsNullOrEmpty(request.WarehouseCen))
        {
            var warehouse = await warehouseRepository.GetByCenAsync(request.WarehouseCen, cancellationToken);
            warehouseId = warehouse?.Id;
        }

        // Get active products
        var products = await productRepository.SearchAsync(
            request.CompanyId,
            request.Search,
            categoryId,
            ProductStatus.Activo,
            cancellationToken
        );

        var result = new List<SellableProductContractDto>();

        foreach (var p in products)
        {
            decimal availableQty = 0;
            if (warehouseId.HasValue)
            {
                var stock = await stockRepository.GetStockByProductAndWarehouseAsync(p.Id, warehouseId.Value, cancellationToken);
                availableQty = stock?.CurrentQuantity ?? 0;
            }
            else
            {
                var stocks = await stockRepository.GetStockByProductIdAsync(p.Id, cancellationToken);
                availableQty = stocks.Sum(s => s.CurrentQuantity);
            }

            if (request.OnlyAvailable && availableQty <= 0)
                continue;

            result.Add(new SellableProductContractDto(
                p.Cen,
                p.Name,
                p.Category.Cen,
                p.Category.Name,
                (double)p.Price,
                (double)availableQty,
                availableQty > 0,
                null // StationCode
            ));
        }

        return result
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();
    }
}
