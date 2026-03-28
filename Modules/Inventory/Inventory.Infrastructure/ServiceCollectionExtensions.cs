using Inventory.Domain.Repositories;
using Inventory.Infrastructure.Endpoints;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInventoryModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Repositorios
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductStockRepository, ProductStockRepository>();
        services.AddScoped<IKardexMovementRepository, KardexMovementRepository>();
        services.AddScoped<IInventoryDocumentRepository, InventoryDocumentRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        
        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Registrar MediatR para la capa Application de Inventory
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssembly(typeof(Application.Features.Products.Queries.GetCompanyProducts.GetCompanyProductsHandler).Assembly);
        });

        return services;
    }

    public static IEndpointRouteBuilder MapInventoryModuleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapInventoryEndpoints();
        app.MapDashboardEndpoints();
        app.MapProductEndpoints();
        app.MapWarehouseEndpoints();
        app.MapCategoryEndpoints();
        app.MapUnitEndpoints();
        
        return app;
    }
}
