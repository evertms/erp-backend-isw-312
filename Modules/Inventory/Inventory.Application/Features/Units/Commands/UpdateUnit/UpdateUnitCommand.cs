using MediatR;

namespace Inventory.Application.Features.Units.Commands.UpdateUnit;

public record UpdateUnitCommand(
    Guid Id,
    Guid CompanyId,
    string Name,
    string Code
) : IRequest<bool>;
