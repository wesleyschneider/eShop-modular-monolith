namespace eShop.Ordering.Module.Application.Commands;

public record CancelOrderCommand(int OrderNumber) : IRequest<bool>;

