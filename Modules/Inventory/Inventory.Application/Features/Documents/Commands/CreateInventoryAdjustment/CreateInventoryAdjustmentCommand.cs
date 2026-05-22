using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Documents.Commands.CreateInventoryAdjustment;

public record CreateInventoryAdjustmentCommand(
    Guid CompanyId,
    InventoryAdjustmentContractRequest Request
) : IRequest<InventoryAdjustmentContractResponse>;
