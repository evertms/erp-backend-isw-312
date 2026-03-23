using MediatR;
using Inventory.Application.Features.Dashboard.DTOs;

namespace Inventory.Application.Features.Dashboard.Queries.GetDashboardMetrics;

public record GetDashboardMetricsQuery(Guid CompanyId) : IRequest<DashboardDto>;
