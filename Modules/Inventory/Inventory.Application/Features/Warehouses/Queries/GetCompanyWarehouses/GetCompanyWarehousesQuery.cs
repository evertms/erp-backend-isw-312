using MediatR;
using Inventory.Application.Features.Warehouses.DTOs;

namespace Inventory.Application.Features.Warehouses.Queries.GetCompanyWarehouses;

public record GetCompanyWarehousesQuery(Guid CompanyId) : IRequest<List<WarehouseDto>>;
