namespace Shared.Contracts.Purchases;

public class CreatePurchaseOrderDto
{
    public required string SupplierCen { get; set; }
    public required string WarehouseCen { get; set; }
    public required List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
}
