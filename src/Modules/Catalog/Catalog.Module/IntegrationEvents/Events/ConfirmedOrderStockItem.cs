namespace eShop.Catalog.Module.IntegrationEvents.Events;

public record ConfirmedOrderStockItem(int ProductId, bool HasStock);
