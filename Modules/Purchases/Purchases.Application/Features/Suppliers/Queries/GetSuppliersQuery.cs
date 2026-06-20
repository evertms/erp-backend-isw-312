using MediatR;
using Purchases.Domain.Repositories;
using Shared.Contracts.Purchases;

namespace Purchases.Application.Features.Suppliers.Queries;

public record GetSuppliersQuery(string CompanyCen) : IRequest<List<SupplierDto>>;

public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, List<SupplierDto>>
{
    private readonly ISupplierRepository _repository;

    public GetSuppliersQueryHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SupplierDto>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await _repository.GetAllByCompanyAsync(request.CompanyCen, cancellationToken);

        return suppliers.Select(s => new SupplierDto
        {
            SupplierCen = s.Cen,
            Name = s.Name
        }).ToList();
    }
}
