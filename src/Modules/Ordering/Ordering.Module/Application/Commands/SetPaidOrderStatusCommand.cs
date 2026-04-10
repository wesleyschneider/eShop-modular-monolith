namespace eShop.Ordering.Module.Application.Commands;

public record SetPaidOrderStatusCommand(int OrderNumber) : IRequest<bool>;
