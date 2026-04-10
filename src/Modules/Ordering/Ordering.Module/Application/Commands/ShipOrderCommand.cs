namespace eShop.Ordering.Module.Application.Commands;

public record ShipOrderCommand(int OrderNumber) : IRequest<bool>;
