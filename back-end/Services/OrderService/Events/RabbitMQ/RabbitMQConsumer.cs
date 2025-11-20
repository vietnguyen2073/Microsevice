using OrderService.Events.Interface;
using OrderService.Events.Models;
using OrderService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using OrderService.DTOs;


namespace PaymentService.Events.RabbitMQ
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IOrderService _orderService;

        public RabbitMQConsumer(IOrderService orderService)
        {
            _orderService = orderService;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("payment_exchange", ExchangeType.Direct, durable: true);
            _channel.QueueDeclare("order_queue", durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind("order_queue", "payment_exchange", "payment.update");
        }

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var orderEvent = JsonConvert.DeserializeObject<OrderCreatedEvent>(message);

                if (orderEvent != null)
                {

                    var paymentDto = new PaymentCreateDto
                    {
                        OrderId = orderEvent.OrderId,
                        Amount = orderEvent.Amount
                    };
                    await _paymentService.CreateAsync(paymentDto);
                }

                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: "payment_queue", autoAck: false, consumer: consumer);
        }
    }
}
