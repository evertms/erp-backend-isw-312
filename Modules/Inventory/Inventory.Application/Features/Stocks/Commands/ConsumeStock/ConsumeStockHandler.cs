using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Domain.Repositories;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Stocks.Commands.ConsumeStock;

public class ConsumeStockHandler(
    IInventoryDocumentRepository documentRepository,
    IProductStockRepository stockRepository,
    IKardexMovementRepository kardexRepository,
    IProductRepository productRepository,
    IWarehouseRepository warehouseRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ConsumeStockCommand, StockConsumeContractResponse>
{
    public async Task<StockConsumeContractResponse> Handle(ConsumeStockCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await warehouseRepository.GetByCenAsync(request.Request.WarehouseCen, cancellationToken);
        if (warehouse == null || warehouse.CompanyCen != request.CompanyCen)
            throw new ArgumentException("Almacén no válido.");

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var document = InventoryDocument.Create(
                request.CompanyCen,
                warehouse.Id,
                DocumentType.Salida,
                DateTime.UtcNow,
                request.Request.Reason ?? $"Consumo desde {request.Request.Source}"
            );

            documentRepository.Add(document);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var requirements = new List<StockRequirementContractDto>();
            
            // First pass: validate all stock
            foreach (var itemRequest in request.Request.Items)
            {
                var product = await productRepository.GetByCenAsync(itemRequest.ProductCen, cancellationToken);
                if (product == null || product.CompanyCen != request.CompanyCen)
                    throw new ArgumentException($"Producto {itemRequest.ProductCen} no válido.");

                var stock = await stockRepository.GetStockByProductAndWarehouseAsync(product.Id, warehouse.Id, cancellationToken);
                var currentStock = stock?.CurrentQuantity ?? 0;
                var quantity = (decimal)itemRequest.Quantity;

                if (currentStock < quantity)
                {
                    requirements.Add(new StockRequirementContractDto(
                        product.Cen,
                        product.Name,
                        warehouse.Cen,
                        (double)quantity,
                        (double)currentStock,
                        (double)(quantity - currentStock),
                        product.Unit.Name ?? "Unidad",
                        "Stock insuficiente"
                    ));
                }
            }

            if (requirements.Any())
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new StockConsumeContractResponse(
                    false,
                    null,
                    null,
                    new List<string>(),
                    requirements
                );
            }

            var movementCens = new List<string>();

            // Second pass: actual consumption
            foreach (var itemRequest in request.Request.Items)
            {
                var product = await productRepository.GetByCenAsync(itemRequest.ProductCen, cancellationToken);
                var quantity = (decimal)itemRequest.Quantity;
                
                document.AddLine(product.Id, quantity);

                var stock = await stockRepository.GetStockByProductAndWarehouseAsync(product.Id, warehouse.Id, cancellationToken);
                if (stock == null)
                {
                    stock = ProductStock.Create(request.CompanyCen, product.Id, warehouse.Id);
                    stockRepository.Add(stock);
                }

                stock.SubtractQuantity(quantity);

                var movement = KardexMovement.Create(
                    request.CompanyCen,
                    product.Id,
                    warehouse.Id,
                    MovementType.Out,
                    quantity,
                    stock.CurrentQuantity + quantity,
                    document.Id,
                    document.Notes
                );
                
                kardexRepository.Add(movement);
                movementCens.Add(movement.Cen);
            }

            document.Confirm();
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return new StockConsumeContractResponse(
                true,
                document.Cen,
                document.Type.ToString(),
                movementCens,
                new List<StockRequirementContractDto>()
            );
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
