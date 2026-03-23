using Core.Domain.Repositories;
using Core.Infrastructure.Endpoints;
using Core.Infrastructure.Persistence;
using Core.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CoreDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssembly(typeof(Application.Features.Companies.Queries.GetActiveCompanies.GetActiveCompaniesHandler).Assembly);
        });

        return services;
    }

    public static IEndpointRouteBuilder MapCoreModuleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCompaniesEndpoints();
        return app;
    }
}
