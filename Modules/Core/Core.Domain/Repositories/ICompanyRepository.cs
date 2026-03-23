using Core.Domain.Entities;

namespace Core.Domain.Repositories;

public interface ICompanyRepository
{
    Task<List<Company>> GetActiveCompaniesAsync(CancellationToken cancellationToken = default);
}
