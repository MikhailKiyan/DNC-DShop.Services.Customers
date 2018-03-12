using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.RabbitMq;
using DShop.Messages.Commands.Customers;
using DShop.Messages.Events.Customers;
using DShop.Services.Customers.Services;

namespace DShop.Services.Customers.Handlers.Customers
{
    public class AddProductToCartHandler : ICommandHandler<AddProductToCart>
    {
        private readonly IBusPublisher _busPublisher;
        private readonly IHandler _handler;
        private readonly ICartService _cartService;

        public AddProductToCartHandler(IBusPublisher busPublisher, 
            IHandler handler, ICartService cartService)
        {
            _busPublisher = busPublisher;
            _handler = handler;
            _cartService = cartService;
        }

        public async Task HandleAsync(AddProductToCart command, ICorrelationContext context)
            => await _handler.Handle(async () => 
                await _cartService.AddProductAsync(context.UserId, command.ProductId, command.Quantity))
                .OnDShopError(async ex => await _busPublisher.PublishEventAsync(
                        new AddProductToCartRejected(context.UserId, command.ProductId,
                            command.Quantity, ex.Message, ex.Code), context)
                )    
                .OnError(async ex => await _busPublisher.PublishEventAsync(
                        new AddProductToCartRejected(context.UserId, command.ProductId,
                            command.Quantity, ex.Message, string.Empty), context)
                )
                .ExecuteAsync();
    }
}