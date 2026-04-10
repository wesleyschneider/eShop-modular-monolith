namespace eShop.Ordering.Module.Application.Commands;
using eShop.Ordering.Module.Application.Models;

public record CreateOrderDraftCommand(string BuyerId, IEnumerable<BasketItem> Items) : IRequest<OrderDraftDTO>;
