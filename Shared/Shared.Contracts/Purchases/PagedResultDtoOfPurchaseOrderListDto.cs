namespace Shared.Contracts.Purchases;

public class PagedResultDtoOfPurchaseOrderListDto
{
    public required List<PurchaseOrderListDto> Items { get; set; } = new();
    public required int TotalCount { get; set; }
    public required int TotalPages { get; set; }
    public required int CurrentPage { get; set; }
}
