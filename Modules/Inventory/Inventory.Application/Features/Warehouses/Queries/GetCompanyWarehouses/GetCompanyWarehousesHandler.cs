using MediatR;
using Inventory.Domain.Repositories;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Warehouses.Queries.GetCompanyWarehouses;

public class GetCompanyWarehousesHandler(IWarehouseRepository warehouseRepository) : IRequestHandler<GetCompanyWarehousesQuery, List<WarehouseContractDto>>
{
    public async Task<List<WarehouseContractDto>> Handle(GetCompanyWarehousesQuery request, CancellationToken cancellationToken)
    {
        var warehouses = await warehouseRepository.GetActiveWarehousesByCompanyIdAsync(request.CompanyId, cancellationToken);
        return warehouses.Select(w => new WarehouseContractDto(w.Id.ToString(), w.Name, true)).ToList();
    }
}
