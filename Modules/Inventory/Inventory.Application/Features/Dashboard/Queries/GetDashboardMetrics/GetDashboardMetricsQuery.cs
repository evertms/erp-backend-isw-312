using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Dashboard.Queries.GetDashboardMetrics;

public record GetDashboardMetricsQuery(Guid CompanyId) : IRequest<InventoryDashboardContractDto>;
