using Shared.Contracts.Inventory;

namespace Sales.Application.Services;

public interface IInventoryIntegrationService
{
    Task<StockValidationContractResponse> ValidateStockAsync(string companyCen, StockValidationContractRequest request, CancellationToken cancellationToken = default);
    Task<StockConsumeContractResponse> ConsumeStockAsync(string companyCen, StockConsumeContractRequest request, CancellationToken cancellationToken = default);
    Task<List<SellableProductContractDto>> GetSellableProductsAsync(string companyCen, string? search = null, string? categoryCen = null, string? warehouseCen = null, bool onlyAvailable = true, int page = 1, int pageSize = 50, CancellationToken cancellationToken = default);
    Task<ProductContractDto?> GetProductByCenAsync(string companyCen, string productCen, CancellationToken cancellationToken = default);
}
