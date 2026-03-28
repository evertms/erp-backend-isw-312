using Inventory.Domain.Repositories;
using MediatR;

namespace Inventory.Application.Features.Products.Commands.UpdateProductStatus;

public class UpdateProductStatusHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateProductStatusCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (product == null || product.CompanyId != request.CompanyId)
        {
            return false;
        }

        product.UpdateStatus(request.Status);

        await productRepository.UpdateAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
