using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Purchases.Application.Services;
using Purchases.Domain.Repositories;
using Purchases.Infrastructure.Integrations;
using Purchases.Infrastructure.Persistence;
using Purchases.Infrastructure.Persistence.Repositories;

namespace Purchases.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPurchasesModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PurchasesDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();

        services.AddHttpClient<IInventoryIntegrationService, InventoryIntegrationService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Features.PurchaseOrders.Commands.CreatePurchaseOrderCommand).Assembly));

        return services;
    }
}
