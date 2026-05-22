using Inventory.Domain.Repositories;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoriesQuery, List<CategoryContractDto>>
{
    public async Task<List<CategoryContractDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetAllCategoriesAsync(request.CompanyCen, cancellationToken);

        return categories
            .Select(c => new CategoryContractDto(c.Cen, c.Name, c.Description, true))
            .ToList();
    }
}
