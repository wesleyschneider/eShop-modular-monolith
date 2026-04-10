namespace eShop.Ordering.Module.Application.Commands;

public record SetStockRejectedOrderStatusCommand(int OrderNumber, List<int> OrderStockItems) : IRequest<bool>;
