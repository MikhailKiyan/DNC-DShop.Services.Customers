using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.RabbitMq;
using DShop.Messages.Events.Orders;
using DShop.Services.Customers.Services;

namespace DShop.Services.Customers.Handlers.Products
{
    public class OrderCompletedHandler : IEventHandler<OrderCompleted>
    {
        private readonly IHandler _handler;
        private readonly ICartService _cartService;

        public OrderCompletedHandler(IHandler handler, 
            ICartService cartService)
        {
            _handler = handler;
            _cartService = cartService;
        }

        public async Task HandleAsync(OrderCompleted @event, ICorrelationContext context)
            => await _handler.Handle(async () => await _cartService.ClearAsync(@event.UserId))
                .ExecuteAsync();
    }
}