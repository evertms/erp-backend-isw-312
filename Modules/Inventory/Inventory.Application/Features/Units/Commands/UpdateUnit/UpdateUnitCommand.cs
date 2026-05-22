using MediatR;

namespace Inventory.Application.Features.Units.Commands.UpdateUnit;

public record UpdateUnitCommand(
    string UnitCen,
    string CompanyCen,
    string Name,
    string Code
) : IRequest<bool>;
