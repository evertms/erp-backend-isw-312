using Inventory.Domain.Repositories;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Stocks.Commands.ValidateStock;

public class ValidateStockHandler(
    IProductStockRepository stockRepository,
    IProductRepository productRepository,
    IWarehouseRepository warehouseRepository) : IRequestHandler<ValidateStockCommand, StockValidationContractResponse>
{
    public async Task<StockValidationContractResponse> Handle(ValidateStockCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await warehouseRepository.GetByCenAsync(request.Request.WarehouseCen, cancellationToken);
        if (warehouse == null || warehouse.CompanyId != request.CompanyId)
            throw new ArgumentException("Almacén no válido.");

        var requirements = new List<StockRequirementContractDto>();
        bool allAvailable = true;

        foreach (var itemRequest in request.Request.Items)
        {
            var product = await productRepository.GetByCenAsync(itemRequest.ProductCen, cancellationToken);
            if (product == null || product.CompanyId != request.CompanyId)
                throw new ArgumentException($"Producto {itemRequest.ProductCen} no válido.");

            var stock = await stockRepository.GetStockByProductAndWarehouseAsync(product.Id, warehouse.Id, cancellationToken);
            var availableQuantity = (double)(stock?.CurrentQuantity ?? 0m);
            var requestedQuantity = itemRequest.Quantity;
            var isAvailable = availableQuantity >= requestedQuantity;

            if (!isAvailable) allAvailable = false;

            requirements.Add(new StockRequirementContractDto(
                product.Cen,
                product.Name,
                warehouse.Cen,
                requestedQuantity,
                availableQuantity,
                Math.Max(0, requestedQuantity - availableQuantity),
                product.Unit?.Name ?? "UN",
                isAvailable ? "Disponible" : "Stock insuficiente"
            ));
        }

        return new StockValidationContractResponse(allAvailable, requirements);
    }
}
