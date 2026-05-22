using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Domain.Repositories;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Documents.Commands.CreateInventoryAdjustment;

public class CreateInventoryAdjustmentHandler(
    IInventoryDocumentRepository documentRepository,
    IProductStockRepository stockRepository,
    IKardexMovementRepository kardexRepository,
    IProductRepository productRepository,
    IWarehouseRepository warehouseRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateInventoryAdjustmentCommand, InventoryAdjustmentContractResponse>
{
    public async Task<InventoryAdjustmentContractResponse> Handle(CreateInventoryAdjustmentCommand request, CancellationToken cancellationToken)
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
                DocumentType.Ajuste,
                DateTime.UtcNow,
                request.Request.Reason
            );

            document.Confirm();
            documentRepository.Add(document);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var generatedMovements = new List<GeneratedMovementContractDto>();

            foreach (var lineRequest in request.Request.Lines)
            {
                var product = await productRepository.GetByCenAsync(lineRequest.ProductCen, cancellationToken);
                if (product == null || product.CompanyCen != request.CompanyCen)
                    throw new ArgumentException($"Producto {lineRequest.ProductCen} no válido.");

                var quantity = (decimal)lineRequest.Quantity;
                document.AddLine(product.Id, quantity);

                var stock = await stockRepository.GetStockByProductAndWarehouseAsync(product.Id, warehouse.Id, cancellationToken);
                if (stock == null)
                {
                    stock = ProductStock.Create(request.CompanyCen, product.Id, warehouse.Id);
                    stockRepository.Add(stock);
                }

                if (!Enum.TryParse<MovementType>(lineRequest.AdjustmentType, true, out var movType))
                {
                    // If adjustmentType is "Entrada"/"Salida", map to In/Out
                    movType = lineRequest.AdjustmentType.ToLower() switch
                    {
                        "entrada" => MovementType.In,
                        "salida" => MovementType.Out,
                        _ => throw new ArgumentException($"Tipo de ajuste no válido: {lineRequest.AdjustmentType}")
                    };
                }

                if (movType == MovementType.In)
                    stock.AddQuantity(quantity);
                else
                    stock.SubtractQuantity(quantity);

                var movement = KardexMovement.Create(
                    request.CompanyCen,
                    product.Id,
                    warehouse.Id,
                    movType,
                    quantity,
                    stock.CurrentQuantity + (movType == MovementType.In ? -quantity : quantity),
                    document.Id,
                    request.Request.Reason
                );
                
                kardexRepository.Add(movement);
                
                generatedMovements.Add(new GeneratedMovementContractDto(
                    movement.Cen,
                    product.Cen,
                    warehouse.Cen,
                    (double)quantity,
                    movType.ToString()
                ));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return new InventoryAdjustmentContractResponse(
                document.Cen,
                document.Status.ToString(),
                generatedMovements
            );
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
