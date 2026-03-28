using Inventory.Application.DTOs;
using Inventory.Domain.Repositories;
using MediatR;

namespace Inventory.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetAllCategoriesAsync(request.CompanyId, cancellationToken);

        return categories
            .Select(c => new CategoryDto(c.Id, c.CompanyId, c.Name, c.Description))
            .ToList();
    }
}
