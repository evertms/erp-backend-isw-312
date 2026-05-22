using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Documents.Commands.CreateInventoryDocument;

public record CreateInventoryDocumentCommand(
    string CompanyCen,
    InventoryDocumentContractRequest Request
) : IRequest<InventoryDocumentContractDto>;
