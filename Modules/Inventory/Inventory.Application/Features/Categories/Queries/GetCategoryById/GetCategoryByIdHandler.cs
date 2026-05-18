using Inventory.Domain.Repositories;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryByIdQuery, CategoryContractDto?>
{
    public async Task<CategoryContractDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category == null || category.CompanyId != request.CompanyId)
        {
            return null;
        }

        return new CategoryContractDto(category.Id.ToString(), category.Name, category.Description, true);
    }
}
