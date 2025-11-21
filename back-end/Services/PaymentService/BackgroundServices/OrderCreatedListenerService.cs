using PaymentService.DTOs;
using PaymentService.Events.Models;
using PaymentService.Services;
using Newtonsoft.Json;

namespace PaymentService.BackgroundServices
{
    public class OrderCreatedListenerService : RabbitMQListenService<OrderCreatedEvent>
    {
        public OrderCreatedListenerService(IServiceProvider serviceProvider)
            : base(serviceProvider, "order_create", "order_exchange", "order.create") { }

        protected override async Task HandleMessage(string rawMessage)
        {
            var orderEvent = JsonConvert.DeserializeObject<OrderCreatedEvent>(rawMessage);
            if (orderEvent != null)
            {
                using var scope = _serviceProvider.CreateScope();
                var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

                var paymentDto = new PaymentCreateDto
                {
                    OrderId = orderEvent.OrderId,
                    Amount = orderEvent.Amount
                };

                await paymentService.CreateAsync(paymentDto);
            }
        }
    }
}
