namespace eShop.Ordering.Module.Application.Commands;

public record SetStockConfirmedOrderStatusCommand(int OrderNumber) : IRequest<bool>;
