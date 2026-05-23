using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Domain.Repositories;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Documents.Commands.CreateInventoryDocument;

public class CreateInventoryDocumentHandler(
    IInventoryDocumentRepository documentRepository,
    IProductStockRepository stockRepository,
    IKardexMovementRepository kardexRepository,
    IProductRepository productRepository,
    IWarehouseRepository warehouseRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateInventoryDocumentCommand, InventoryDocumentContractDto>
{
    public async Task<InventoryDocumentContractDto> Handle(CreateInventoryDocumentCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await warehouseRepository.GetByCenAsync(request.Request.WarehouseCen, cancellationToken);
        if (warehouse == null || warehouse.CompanyCen != request.CompanyCen)
            throw new ArgumentException("Almacén no válido.");

        if (!Enum.TryParse<DocumentType>(request.Request.DocumentType, true, out var docType))
            throw new ArgumentException("Tipo de documento no válido.");

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var document = InventoryDocument.Create(
                request.CompanyCen,
                warehouse.Id,
                docType,
                DateTime.UtcNow,
                request.Request.Reason
            );

            documentRepository.Add(document);
            await unitOfWork.SaveChangesAsync(cancellationToken); // Get Document.Id

            var movements = new List<KardexMovement>();

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

                MovementType movType = docType switch
                {
                    DocumentType.Entrada => MovementType.In,
                    DocumentType.Salida => MovementType.Out,
                    _ => throw new InvalidOperationException("Solo se soportan Entradas y Salidas en este endpoint por ahora.")
                };

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
                    stock.CurrentQuantity + (movType == MovementType.In ? -quantity : quantity), // Balance before this movement
                    document.Id,
                    request.Request.Reason
                );
                
                kardexRepository.Add(movement);
                movements.Add(movement);
            }

            document.Confirm();
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return new InventoryDocumentContractDto(
                document.Cen,
                document.Type.ToString(),
                document.Status.ToString(),
                $"Documento {document.Cen}",
                document.CreatedAt,
                document.Lines.Count,
                movements.Select(m => m.Cen).ToList()
            );
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
