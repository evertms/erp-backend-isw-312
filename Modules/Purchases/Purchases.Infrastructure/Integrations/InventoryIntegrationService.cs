using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Purchases.Application.Services;
using Shared.Contracts.Inventory;

namespace Purchases.Infrastructure.Integrations;

public class InventoryIntegrationService : IInventoryIntegrationService
{
    private readonly HttpClient _httpClient;
    private readonly string _inventoryApiUrl;

    public InventoryIntegrationService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _inventoryApiUrl = configuration["InventoryApiUrl"] ?? "http://localhost:5229"; // Default base URL
    }

    public async Task UpdateInventoryFromPurchaseAsync(string companyCen, string warehouseCen, List<(string ProductCen, int Quantity)> items, CancellationToken cancellationToken = default)
    {
        var request = new InventoryDocumentContractRequest(
            DocumentType: "PurchaseReceipt",
            WarehouseCen: warehouseCen,
            Reason: "Compra de inventario",
            ExternalReference: null,
            Lines: items.Select(x => new InventoryDocumentLineContractRequest(
                ProductCen: x.ProductCen,
                Quantity: x.Quantity,
                UnitCost: null
            )).ToList()
        );

        var url = $"{_inventoryApiUrl}/api/inventory/companies/{companyCen}/documents";
        
        var response = await _httpClient.PostAsJsonAsync(url, request, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"Failed to update inventory. Status Code: {response.StatusCode}. Details: {content}");
        }
    }
}
