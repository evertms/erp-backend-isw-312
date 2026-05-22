using Microsoft.Extensions.DependencyInjection;

namespace Purchases.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPurchasesModule(this IServiceCollection services)
    {
        // Aquí registraremos los repositorios, DbContext y servicios de Purchases
        // services.AddScoped<ITicketRepository, TicketRepository>();
        
        // Registrar MediatR para la capa Application de Purchases (comentado hasta que haya features)
        // services.AddMediatR(config => 
        // {
        //     config.RegisterServicesFromAssembly(typeof(Application.SomeFeature.Handler).Assembly);
        // });

        return services;
    }
}
