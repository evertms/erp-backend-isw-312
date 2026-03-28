using Inventory.Domain.Repositories;
using MediatR;
using Unit = Inventory.Domain.Entities.Unit;

namespace Inventory.Application.Features.Units.Commands.CreateUnit;

public class CreateUnitHandler(
    IUnitRepository unitRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateUnitCommand, Guid>
{
    public async Task<Guid> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        // Validación de duplicidad (Criterio de Aceptación 2)
        var isNameUnique = await unitRepository.IsNameUniqueAsync(request.CompanyId, request.Name, cancellationToken);
        if (!isNameUnique)
        {
            throw new InvalidOperationException("Ya existe una unidad de medida con ese nombre.");
        }

        var isCodeUnique = await unitRepository.IsCodeUniqueAsync(request.CompanyId, request.Code, cancellationToken);
        if (!isCodeUnique)
        {
            throw new InvalidOperationException("Ya existe una unidad de medida con ese código.");
        }

        var unit = Unit.Create(request.CompanyId, request.Name, request.Code);

        await unitRepository.AddAsync(unit, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return unit.Id;
    }
}
