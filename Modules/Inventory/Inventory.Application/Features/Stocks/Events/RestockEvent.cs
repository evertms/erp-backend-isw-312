namespace Inventory.Application.Features.Stocks.Events;

public record RestockEvent(string Producto, decimal Cantidad);
