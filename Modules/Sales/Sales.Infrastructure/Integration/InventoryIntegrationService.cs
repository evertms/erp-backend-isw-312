using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sales.Application.Services;
using Shared.Contracts.Inventory;
using Polly;

namespace Sales.Infrastructure.Integration;

public class InventoryIntegrationService(HttpClient httpClient) : IInventoryIntegrationService
{
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<StockValidationContractResponse> ValidateStockAsync(string companyCen, StockValidationContractRequest request, CancellationToken cancellationToken = default)
    {
        var fallback = Policy<StockValidationContractResponse>
            .Handle<HttpRequestException>()
            .FallbackAsync(new StockValidationContractResponse(
                IsValid: false,
                Requirements: new List<StockRequirementContractDto> {
                    new StockRequirementContractDto("N/A", "Inventario no disponible temporalmente", "N/A", 0, 0, 0, "N/A", "Fallback Polly")
                }
            ));

        return await fallback.ExecuteAsync(async () =>
        {
            var response = await httpClient.PostAsJsonAsync($"/api/inventory/companies/{companyCen}/stock/validate", request, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<StockValidationContractResponse>(_jsonOptions, cancellationToken);
            return result ?? throw new InvalidOperationException("Failed to deserialize stock validation response.");
        });
    }

    public async Task<StockConsumeContractResponse> ConsumeStockAsync(string companyCen, StockConsumeContractRequest request, CancellationToken cancellationToken = default)
    {
        var fallback = Policy<StockConsumeContractResponse>
            .Handle<HttpRequestException>()
            .FallbackAsync(new StockConsumeContractResponse(
                Success: false,
                DocumentCen: null,
                DocumentType: null,
                GeneratedMovementCens: new List<string>(),
                Requirements: new List<StockRequirementContractDto> {
                    new StockRequirementContractDto("N/A", "Inventario no disponible temporalmente", "N/A", 0, 0, 0, "N/A", "Fallback Polly")
                }
            ));

        return await fallback.ExecuteAsync(async () =>
        {
            var response = await httpClient.PostAsJsonAsync($"/api/inventory/companies/{companyCen}/stock/consume", request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<StockConsumeContractResponse>(_jsonOptions, cancellationToken);
            return result ?? throw new InvalidOperationException("Failed to deserialize stock consume response.");
        });
    }

    public async Task<List<SellableProductContractDto>> GetSellableProductsAsync(string companyCen, string? search = null, string? categoryCen = null, string? warehouseCen = null, bool onlyAvailable = true, int page = 1, int pageSize = 50, CancellationToken cancellationToken = default)
    {
        var url = $"/api/inventory/companies/{companyCen}/sellable-products?search={search}&categoryCen={categoryCen}&warehouseCen={warehouseCen}&onlyAvailable={onlyAvailable}&page={page}&pageSize={pageSize}";
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<SellableProductContractDto>>(_jsonOptions, cancellationToken);
        return result ?? new List<SellableProductContractDto>();
    }

    public async Task<ProductContractDto?> GetProductByCenAsync(string companyCen, string productCen, CancellationToken cancellationToken = default)
    {
        var request = new ProductLookupContractRequest(new List<string> { productCen });
        var response = await httpClient.PostAsJsonAsync($"/api/inventory/companies/{companyCen}/products/lookup", request, cancellationToken);
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<List<ProductContractDto>>(_jsonOptions, cancellationToken);
        return products?.FirstOrDefault();
    }
}
