using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;
using Sales.Domain.Repositories;

namespace Sales.Infrastructure.Persistence.Repositories;

public class TicketRepository(SalesDbContext dbContext) : ITicketRepository
{
    public async Task<Ticket?> GetByCenAsync(string cen, CancellationToken cancellationToken = default)
    {
        return await dbContext.Tickets
            .Include(t => t.Lines)
            .Include(t => t.Payments)
            .FirstOrDefaultAsync(t => t.Cen == cen, cancellationToken);
    }

    public async Task<List<Ticket>> GetDailyTicketsAsync(string companyCen, CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        return await dbContext.Tickets
            .Include(t => t.Lines)
            .Where(t => t.CompanyCen == companyCen && t.CreatedAt >= today)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default)
    {
        await dbContext.Tickets.AddAsync(ticket, cancellationToken);
    }

    public Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken = default)
    {
        dbContext.Tickets.Update(ticket);
        return Task.CompletedTask;
    }

    public async Task<int> GetNextDailyNumberAsync(string companyCen, CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var lastNumber = await dbContext.Tickets
            .Where(t => t.CompanyCen == companyCen && t.CreatedAt >= today)
            .MaxAsync(t => (int?)t.DailyNumber, cancellationToken) ?? 0;
            
        return lastNumber + 1;
    }
}

public class TaxConfigurationRepository(SalesDbContext dbContext) : ITaxConfigurationRepository
{
    public async Task<TaxConfiguration?> GetByCompanyCenAsync(string companyCen, CancellationToken cancellationToken = default)
    {
        return await dbContext.TaxConfigurations
            .FirstOrDefaultAsync(t => t.CompanyCen == companyCen, cancellationToken);
    }

    public async Task AddAsync(TaxConfiguration config, CancellationToken cancellationToken = default)
    {
        await dbContext.TaxConfigurations.AddAsync(config, cancellationToken);
    }

    public Task UpdateAsync(TaxConfiguration config, CancellationToken cancellationToken = default)
    {
        dbContext.TaxConfigurations.Update(config);
        return Task.CompletedTask;
    }
}

public class TicketLineRepository(SalesDbContext dbContext) : ITicketLineRepository
{
    public async Task<TicketLine?> GetByCenAsync(string cen, CancellationToken cancellationToken = default)
    {
        return await dbContext.TicketLines
            .FirstOrDefaultAsync(l => l.Cen == cen, cancellationToken);
    }

    public Task UpdateAsync(TicketLine line, CancellationToken cancellationToken = default)
    {
        dbContext.TicketLines.Update(line);
        return Task.CompletedTask;
    }
}

public class StationCategoryConfigRepository(SalesDbContext dbContext) : IStationCategoryConfigRepository
{
    public async Task<List<StationCategoryConfig>> GetByCompanyCenAsync(string companyCen, CancellationToken cancellationToken = default)
    {
        return await dbContext.StationCategoryConfigs
            .Where(s => s.CompanyCen == companyCen)
            .ToListAsync(cancellationToken);
    }

    public async Task<StationCategoryConfig?> GetByCenAsync(string cen, CancellationToken cancellationToken = default)
    {
        return await dbContext.StationCategoryConfigs
            .FirstOrDefaultAsync(s => s.Cen == cen, cancellationToken);
    }
}

public class UnitOfWork(SalesDbContext dbContext) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (dbContext.Database.CurrentTransaction != null)
        {
            await dbContext.Database.CurrentTransaction.CommitAsync(cancellationToken);
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (dbContext.Database.CurrentTransaction != null)
        {
            await dbContext.Database.CurrentTransaction.RollbackAsync(cancellationToken);
        }
    }
}
