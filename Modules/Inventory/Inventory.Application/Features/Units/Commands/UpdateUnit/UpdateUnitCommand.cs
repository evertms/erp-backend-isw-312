using MediatR;

namespace Inventory.Application.Features.Units.Commands.UpdateUnit;

public record UpdateUnitCommand(
    string UnitCen,
    Guid CompanyId,
    string Name,
    string Code
) : IRequest<bool>;
