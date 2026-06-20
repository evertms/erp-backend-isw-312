using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;
using Sales.Domain.Repositories;

namespace Sales.Infrastructure.Persistence.Repositories;

public class SalesConfigurationRepository(SalesDbContext dbContext) : ISalesConfigurationRepository
{
    public async Task<SalesConfiguration?> GetByCompanyCenAsync(string companyCen, CancellationToken cancellationToken = default)
    {
        return await dbContext.SalesConfigurations
            .FirstOrDefaultAsync(x => x.CompanyCen == companyCen, cancellationToken);
    }

    public async Task AddAsync(SalesConfiguration config, CancellationToken cancellationToken = default)
    {
        await dbContext.SalesConfigurations.AddAsync(config, cancellationToken);
    }

    public Task UpdateAsync(SalesConfiguration config, CancellationToken cancellationToken = default)
    {
        dbContext.SalesConfigurations.Update(config);
        return Task.CompletedTask;
    }
}
