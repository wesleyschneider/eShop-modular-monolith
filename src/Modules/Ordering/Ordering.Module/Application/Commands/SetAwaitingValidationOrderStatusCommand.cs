namespace eShop.Ordering.Module.Application.Commands;

public record SetAwaitingValidationOrderStatusCommand(int OrderNumber) : IRequest<bool>;
