using MediatR;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Waiters.Queries.GetWaiters;

public record GetWaitersQuery(string CompanyCen) : IRequest<List<WaiterContractResponse>>;

public class GetWaitersHandler : IRequestHandler<GetWaitersQuery, List<WaiterContractResponse>>
{
    public Task<List<WaiterContractResponse>> Handle(GetWaitersQuery request, CancellationToken cancellationToken)
    {
        // En una implementación real, esto consultaría a un módulo de recursos humanos o usuarios (Core)
        var waiters = new List<WaiterContractResponse>
        {
            new("W-001", "Juan Perez"),
            new("W-002", "Maria Lopez")
        };

        return Task.FromResult(waiters);
    }
}
