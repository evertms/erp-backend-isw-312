using MediatR;
using Inventory.Domain.Repositories;
using Shared.Contracts.Inventory;
using Inventory.Domain.Enums;

namespace Inventory.Application.Features.Documents.Queries.GetInventoryDocuments;

public class GetInventoryDocumentsHandler(IInventoryDocumentRepository documentRepository) : IRequestHandler<GetInventoryDocumentsQuery, List<InventoryDocumentContractDto>>
{
    public async Task<List<InventoryDocumentContractDto>> Handle(GetInventoryDocumentsQuery request, CancellationToken cancellationToken)
    {
        DocumentType? typeEnum = null;
        if (!string.IsNullOrEmpty(request.DocumentType) && Enum.TryParse<DocumentType>(request.DocumentType, true, out var parsedType))
        {
            typeEnum = parsedType;
        }

        var documents = await documentRepository.GetDocumentsAsync(
            request.CompanyCen,
            typeEnum,
            request.From,
            request.To,
            cancellationToken
        );

        return documents.Select(d => new InventoryDocumentContractDto(
            d.Cen,
            d.Type.ToString(),
            d.Status.ToString(),
            $"Documento {d.Cen}",
            d.CreatedAt,
            d.Lines.Count,
            new List<string>(), // generatedMovementCens not stored in document entity directly in this implementation
            d.Lines.Count > 1 ? "Varios productos" : d.Lines.FirstOrDefault()?.Product?.Name,
            d.Warehouse?.Name,
            (double)d.Lines.Sum(l => l.Quantity),
            d.Lines.Select(l => new InventoryDocumentLineContractDto(
                l.Product?.Cen ?? l.ProductId.ToString(),
                l.Product?.Name ?? l.Product?.Cen ?? l.ProductId.ToString(),
                (double)l.Quantity
            )).ToList()
        )).ToList();
    }
}
