using Sales.Domain.Entities;

namespace Sales.Domain.Repositories;

public interface ITicketRepository
{
    Task<Ticket?> GetByCenAsync(string cen, CancellationToken cancellationToken = default);
    Task<List<Ticket>> GetDailyTicketsAsync(string companyCen, CancellationToken cancellationToken = default);
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default);
    Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken = default);
    Task<int> GetNextDailyNumberAsync(string companyCen, CancellationToken cancellationToken = default);
}

public interface ITaxConfigurationRepository
{
    Task<TaxConfiguration?> GetByCompanyCenAsync(string companyCen, CancellationToken cancellationToken = default);
    Task AddAsync(TaxConfiguration config, CancellationToken cancellationToken = default);
    Task UpdateAsync(TaxConfiguration config, CancellationToken cancellationToken = default);
}

public interface ITicketLineRepository
{
    Task<TicketLine?> GetByCenAsync(string cen, CancellationToken cancellationToken = default);
    Task UpdateAsync(TicketLine line, CancellationToken cancellationToken = default);
}

public interface IStationCategoryConfigRepository
{
    Task<List<StationCategoryConfig>> GetByCompanyCenAsync(string companyCen, CancellationToken cancellationToken = default);
    Task<StationCategoryConfig?> GetByCenAsync(string cen, CancellationToken cancellationToken = default);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
