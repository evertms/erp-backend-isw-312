using Inventory.Application.DTOs;
using Inventory.Domain.Repositories;
using MediatR;

namespace Inventory.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category == null || category.CompanyId != request.CompanyId)
        {
            return null;
        }

        return new CategoryDto(category.Id, category.CompanyId, category.Name, category.Description);
    }
}
