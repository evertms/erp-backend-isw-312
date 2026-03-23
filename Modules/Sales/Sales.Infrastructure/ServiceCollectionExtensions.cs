using Microsoft.Extensions.DependencyInjection;

namespace Sales.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSalesModule(this IServiceCollection services)
    {
        // Aquí registraremos los repositorios, DbContext y servicios de Sales
        // services.AddScoped<ITicketRepository, TicketRepository>();
        
        // Registrar MediatR para la capa Application de Sales (comentado hasta que haya features)
        // services.AddMediatR(config => 
        // {
        //     config.RegisterServicesFromAssembly(typeof(Application.SomeFeature.Handler).Assembly);
        // });

        return services;
    }
}
