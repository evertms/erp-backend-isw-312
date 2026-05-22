using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Products.Queries.ProductLookup;

public record ProductLookupQuery(
    Guid CompanyId,
    ProductLookupContractRequest Request
) : IRequest<List<ProductContractDto>>;
