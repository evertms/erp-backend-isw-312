using Inventory.Domain.Repositories;
using MediatR;

namespace Inventory.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IUnitRepository unitRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductCommand, bool>
{
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (product == null || product.CompanyId != request.CompanyId)
        {
            return false;
        }

        // Validate Category if changed
        if (product.CategoryId != request.CategoryId)
        {
            var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            if (category == null || category.CompanyId != request.CompanyId)
            {
                throw new ArgumentException("La categoría especificada no existe o no pertenece a la empresa.");
            }
        }

        // Validate Unit if changed
        if (product.UnitId != request.UnitId)
        {
            var unit = await unitRepository.GetByIdAsync(request.UnitId, cancellationToken);
            if (unit == null || unit.CompanyId != request.CompanyId)
            {
                throw new ArgumentException("La unidad de medida especificada no existe o no pertenece a la empresa.");
            }
        }

        product.Update(
            request.Name,
            request.CategoryId,
            request.UnitId,
            request.Price,
            request.Code,
            request.SupplierId,
            request.ImageUrl,
            request.MinStockAlert
        );

        await productRepository.UpdateAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
