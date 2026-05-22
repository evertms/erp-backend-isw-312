using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Sales.Application.Services;
using Sales.Domain.Repositories;
using Sales.Infrastructure.Integration;
using Sales.Infrastructure.Persistence;
using Sales.Infrastructure.Persistence.Repositories;

namespace Sales.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSalesModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SalesDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ITicketLineRepository, TicketLineRepository>();
        services.AddScoped<ITaxConfigurationRepository, TaxConfigurationRepository>();
        services.AddScoped<IStationCategoryConfigRepository, StationCategoryConfigRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Integration Service with Resilient HttpClient
        var inventoryUrl = configuration["PARTNER_INVENTORY_URL"] ?? "http://localhost:5239";
        
        services.AddHttpClient<IInventoryIntegrationService, InventoryIntegrationService>(client =>
        {
            client.BaseAddress = new Uri(inventoryUrl);
        })
        .AddPolicyHandler(GetRetryPolicy());

        // Registrar MediatR para la capa Application de Sales
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssembly(typeof(Application.Class1).Assembly);
        });

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) // Optional: decided by business if 404 is transient
            .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
