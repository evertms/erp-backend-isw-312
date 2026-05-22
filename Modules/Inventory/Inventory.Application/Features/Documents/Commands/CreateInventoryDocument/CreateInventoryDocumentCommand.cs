using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Documents.Commands.CreateInventoryDocument;

public record CreateInventoryDocumentCommand(
    Guid CompanyId,
    InventoryDocumentContractRequest Request
) : IRequest<InventoryDocumentContractDto>;
