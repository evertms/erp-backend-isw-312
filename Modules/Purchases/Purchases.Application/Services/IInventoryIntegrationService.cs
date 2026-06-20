namespace Purchases.Application.Services;

public interface IInventoryIntegrationService
{
    Task UpdateInventoryFromPurchaseAsync(string companyCen, string warehouseCen, List<(string ProductCen, int Quantity)> items, CancellationToken cancellationToken = default);
}
