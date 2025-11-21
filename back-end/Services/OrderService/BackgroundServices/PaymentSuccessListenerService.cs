using Newtonsoft.Json;
using OrderService.DTOs.OrderDTOs;
using OrderService.Events.Models;
using OrderService.Services;

namespace OrderService.BackgroundServices
{
    public class PaymentSuccessListenerService: RabbitMQListenService<PaymentCompletedEvent>
    {
        public PaymentSuccessListenerService(IServiceProvider serviceProvider)
           : base(serviceProvider, "payment_update_status", "payment_exchange", "payment.update") { }

        protected override async Task HandleMessage(string rawMessage)
        {
            var paymentCompletedEvent = JsonConvert.DeserializeObject<PaymentCompletedEvent>(rawMessage);
            if (paymentCompletedEvent != null)
            {
                using var scope = _serviceProvider.CreateScope();
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                var orderUpdatetDto = new OrderUpdateDto
                {
                    Status = paymentCompletedEvent.Status
                };

                await orderService.UpdateAsync(paymentCompletedEvent.OrderId, orderUpdatetDto);
            }
        }
    }
}
