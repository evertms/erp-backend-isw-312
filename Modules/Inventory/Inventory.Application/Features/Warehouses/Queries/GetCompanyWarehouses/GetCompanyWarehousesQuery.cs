using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Warehouses.Queries.GetCompanyWarehouses;

public record GetCompanyWarehousesQuery(Guid CompanyId) : IRequest<List<WarehouseContractDto>>;
