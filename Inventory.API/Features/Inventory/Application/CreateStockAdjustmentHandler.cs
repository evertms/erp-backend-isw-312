using Inventory.API.Shared.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Features.Inventory.Application;

public class CreateStockAdjustmentHandler
{
    private readonly AppDbContext _db;

    public CreateStockAdjustmentHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> HandleAsync(CreateStockAdjustmentRequest request)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            var documentId = Guid.NewGuid();
            var document = new InventoryDocument
            {
                Id = documentId,
                CompanyId = request.CompanyId,
                WarehouseId = request.WarehouseId,
                Type = "Ajuste", // Indicates this document is a stock adjustment
                Status = "Confirmado", // Immediately confirmed to reflect changes in kardex and stock
                Notes = request.Notes
            };

            _db.InventoryDocuments.Add(document);

            // Process each line in the adjustment request
            foreach (var line in request.Lines)
            {
                var docLine = new InventoryDocumentLine
                {
                    Id = Guid.NewGuid(),
                    DocumentId = documentId,
                    ProductId = line.ProductId,
                    Quantity = line.QuantityDifference
                };

                _db.InventoryDocumentLines.Add(docLine);

                // Check for existing stock record
                var stock = await _db.ProductStocks
                    .FirstOrDefaultAsync(ps => ps.CompanyId == request.CompanyId &&
                                               ps.WarehouseId == request.WarehouseId &&
                                               ps.ProductId == line.ProductId);

                if (stock == null)
                {
                    // Create stock entry if it doesn't exist
                    stock = new ProductStock
                    {
                        Id = Guid.NewGuid(),
                        CompanyId = request.CompanyId,
                        WarehouseId = request.WarehouseId,
                        ProductId = line.ProductId,
                        CurrentQuantity = line.QuantityDifference
                    };
                    _db.ProductStocks.Add(stock);
                }
                else
                {
                    // Update current stock quantity
                    stock.CurrentQuantity += line.QuantityDifference;
                    _db.ProductStocks.Update(stock);
                }

                // Generates movement strictly for tracebility in the Kardex
                var kardex = new KardexMovement
                {
                    Id = Guid.NewGuid(),
                    CompanyId = request.CompanyId,
                    WarehouseId = request.WarehouseId,
                    ProductId = line.ProductId,
                    DocumentId = documentId,
                    MovementType = "Ajuste",
                    Quantity = line.QuantityDifference,
                    Balance = stock.CurrentQuantity,
                    Reason = request.Notes
                };

                _db.KardexMovements.Add(kardex);
            }

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return documentId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
