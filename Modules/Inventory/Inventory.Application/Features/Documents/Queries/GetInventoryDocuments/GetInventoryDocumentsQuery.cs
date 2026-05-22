using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Documents.Queries.GetInventoryDocuments;

public record GetInventoryDocumentsQuery(
    string CompanyCen,
    string? DocumentType = null,
    DateTime? From = null,
    DateTime? To = null
) : IRequest<List<InventoryDocumentContractDto>>;
