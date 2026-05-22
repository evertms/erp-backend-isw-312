using Inventory.Domain.Repositories;
using MediatR;

namespace Inventory.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateCategoryCommand, bool>
{
    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByCenAsync(request.CategoryCen, cancellationToken);

        if (category == null || category.CompanyCen != request.CompanyCen)
        {
            return false; // NotFound or Unauthorized
        }

        category.Update(request.Name, request.Description);

        await categoryRepository.UpdateAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
