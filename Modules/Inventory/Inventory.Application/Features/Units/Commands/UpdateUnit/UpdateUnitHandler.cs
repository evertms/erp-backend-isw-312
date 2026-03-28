using Inventory.Domain.Repositories;
using MediatR;

namespace Inventory.Application.Features.Units.Commands.UpdateUnit;

public class UpdateUnitHandler(
    IUnitRepository unitRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateUnitCommand, bool>
{
    public async Task<bool> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await unitRepository.GetByIdAsync(request.Id, cancellationToken);

        if (unit == null || unit.CompanyId != request.CompanyId)
        {
            return false;
        }

        // Validar duplicidad en nombre si lo cambió
        if (unit.Name != request.Name)
        {
            var isNameUnique = await unitRepository.IsNameUniqueAsync(request.CompanyId, request.Name, cancellationToken);
            if (!isNameUnique)
            {
                throw new InvalidOperationException("Ya existe otra unidad de medida con ese nombre.");
            }
        }

        // Validar duplicidad en código si lo cambió
        if (unit.Code != request.Code)
        {
            var isCodeUnique = await unitRepository.IsCodeUniqueAsync(request.CompanyId, request.Code, cancellationToken);
            if (!isCodeUnique)
            {
                throw new InvalidOperationException("Ya existe otra unidad de medida con ese código.");
            }
        }

        unit.Update(request.Name, request.Code);

        await unitRepository.UpdateAsync(unit, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
