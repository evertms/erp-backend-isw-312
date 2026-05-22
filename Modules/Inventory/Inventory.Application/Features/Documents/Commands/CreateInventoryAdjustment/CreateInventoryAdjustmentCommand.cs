using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Documents.Commands.CreateInventoryAdjustment;

public record CreateInventoryAdjustmentCommand(
    string CompanyCen,
    InventoryAdjustmentContractRequest Request
) : IRequest<InventoryAdjustmentContractResponse>;
