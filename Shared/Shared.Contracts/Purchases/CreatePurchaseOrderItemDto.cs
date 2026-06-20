namespace Shared.Contracts.Purchases;

public class CreatePurchaseOrderItemDto
{
    public required string ProductCen { get; set; }
    public required int Quantity { get; set; }
}
