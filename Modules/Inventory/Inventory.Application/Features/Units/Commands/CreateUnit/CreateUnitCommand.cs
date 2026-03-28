using MediatR;

namespace Inventory.Application.Features.Units.Commands.CreateUnit;

public record CreateUnitCommand(
    Guid CompanyId,
    string Name,
    string Code
) : IRequest<Guid>;
