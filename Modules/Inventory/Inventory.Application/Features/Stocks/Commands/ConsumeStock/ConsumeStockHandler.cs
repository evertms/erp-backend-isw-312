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
        if (warehouse == null || warehouse.CompanyId != request.CompanyId)
            throw new ArgumentException("Almacén no válido.");

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var document = InventoryDocument.Create(
                request.CompanyId,
                warehouse.Id,
                DocumentType.Salida,
                DateTime.UtcNow,
                request.Request.Reason ?? $"Consumo desde {request.Request.Source}"
            );

            document.Confirm();
            documentRepository.Add(document);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var movementCens = new List<string>();

            foreach (var itemRequest in request.Request.Items)
            {
                var product = await productRepository.GetByCenAsync(itemRequest.ProductCen, cancellationToken);
                if (product == null || product.CompanyId != request.CompanyId)
                    throw new ArgumentException($"Producto {itemRequest.ProductCen} no válido.");

                var quantity = (decimal)itemRequest.Quantity;
                document.AddLine(product.Id, quantity);

                var stock = await stockRepository.GetStockByProductAndWarehouseAsync(product.Id, warehouse.Id, cancellationToken);
                if (stock == null)
                {
                    stock = ProductStock.Create(request.CompanyId, product.Id, warehouse.Id);
                    stockRepository.Add(stock);
                }

                stock.SubtractQuantity(quantity);

                var movement = KardexMovement.Create(
                    request.CompanyId,
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
