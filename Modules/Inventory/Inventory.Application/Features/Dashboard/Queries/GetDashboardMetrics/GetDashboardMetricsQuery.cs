using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Dashboard.Queries.GetDashboardMetrics;

public record GetDashboardMetricsQuery(string CompanyCen) : IRequest<InventoryDashboardContractDto>;
