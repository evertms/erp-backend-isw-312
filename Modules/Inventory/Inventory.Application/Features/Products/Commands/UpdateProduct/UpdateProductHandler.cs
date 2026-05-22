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
        var product = await productRepository.GetByCenAsync(request.ProductCen, cancellationToken);
        
        if (product == null || product.CompanyCen != request.CompanyCen)
        {
            return false;
        }

        // Validate Category if changed
        var category = await categoryRepository.GetByCenAsync(request.CategoryCen, cancellationToken);
        if (category == null || category.CompanyCen != request.CompanyCen)
        {
            throw new ArgumentException("La categoría especificada no existe o no pertenece a la empresa.");
        }

        // Validate Unit if changed
        var unit = await unitRepository.GetByCenAsync(request.UnitCen, cancellationToken);
        if (unit == null || unit.CompanyCen != request.CompanyCen)
        {
            throw new ArgumentException("La unidad de medida especificada no existe o no pertenece a la empresa.");
        }

        product.Update(
            request.Name,
            category.Id,
            unit.Id,
            request.Price,
            request.Code,
            null, // TODO: Map
            request.ImageUrl,
            request.MinStockAlert
        );

        await productRepository.UpdateAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
