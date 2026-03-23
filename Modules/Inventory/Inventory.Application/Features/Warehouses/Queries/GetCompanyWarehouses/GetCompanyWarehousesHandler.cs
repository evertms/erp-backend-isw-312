using MediatR;
using Inventory.Application.Features.Warehouses.DTOs;
using Inventory.Domain.Repositories;

namespace Inventory.Application.Features.Warehouses.Queries.GetCompanyWarehouses;

public class GetCompanyWarehousesHandler(IWarehouseRepository warehouseRepository) : IRequestHandler<GetCompanyWarehousesQuery, List<WarehouseDto>>
{
    public async Task<List<WarehouseDto>> Handle(GetCompanyWarehousesQuery request, CancellationToken cancellationToken)
    {
        var warehouses = await warehouseRepository.GetActiveWarehousesByCompanyIdAsync(request.CompanyId, cancellationToken);
        return warehouses.Select(w => new WarehouseDto(w.Id, w.Name, w.Location)).ToList();
    }
}
