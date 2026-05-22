using MediatR;

namespace Inventory.Application.Features.Units.Commands.CreateUnit;

public record CreateUnitCommand(
    string CompanyCen,
    string Name,
    string Code
) : IRequest<string>;
