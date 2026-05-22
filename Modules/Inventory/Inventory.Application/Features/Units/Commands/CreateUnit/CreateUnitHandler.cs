using Inventory.Domain.Repositories;
using MediatR;
using Unit = Inventory.Domain.Entities.Unit;

namespace Inventory.Application.Features.Units.Commands.CreateUnit;

public class CreateUnitHandler(
    IUnitRepository unitRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateUnitCommand, string>
{
    public async Task<string> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        // Validación de duplicidad (Criterio de Aceptación 2)
        var isNameUnique = await unitRepository.IsNameUniqueAsync(request.CompanyCen, request.Name, cancellationToken);
        if (!isNameUnique)
        {
            throw new InvalidOperationException("Ya existe una unidad de medida con ese nombre.");
        }

        var isCodeUnique = await unitRepository.IsCodeUniqueAsync(request.CompanyCen, request.Code, cancellationToken);
        if (!isCodeUnique)
        {
            throw new InvalidOperationException("Ya existe una unidad de medida con ese código.");
        }

        var unit = Unit.Create(request.CompanyCen, request.Name, request.Code);

        await unitRepository.AddAsync(unit, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return unit.Cen;
    }
}
