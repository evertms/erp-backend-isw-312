using Inventory.Application.DTOs;
using MediatR;

namespace Inventory.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id, Guid CompanyId) : IRequest<ProductDetailsDto?>;
