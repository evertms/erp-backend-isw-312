using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using MediatR;

namespace Inventory.Application.Features.Products.Commands.CreateProduct;

public class CreateProductHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IUnitRepository unitRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCommand, string>
{
    public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Validación de existencia de Categoría
        var category = await categoryRepository.GetByCenAsync(request.CategoryCen, cancellationToken);
        if (category == null || category.CompanyId != request.CompanyId)
        {
            throw new ArgumentException("La categoría especificada no existe o no pertenece a la empresa.");
        }

        // Validación de existencia de Unidad de medida
        var unit = await unitRepository.GetByCenAsync(request.UnitCen, cancellationToken);
        if (unit == null || unit.CompanyId != request.CompanyId)
        {
            throw new ArgumentException("La unidad de medida especificada no existe o no pertenece a la empresa.");
        }

        // El dominio se encarga de validar: nombre, price > 0, etc.
        var product = Product.Create(
            request.CompanyId,
            category.Id,
            unit.Id,
            request.Name,
            request.Price,
            request.Code,
            null, // TODO: Map SupplierCen if needed
            request.ImageUrl,
            request.MinStockAlert
        );

        await productRepository.AddAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Cen;
    }
}
