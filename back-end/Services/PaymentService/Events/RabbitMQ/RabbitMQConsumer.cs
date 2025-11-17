using PaymentService.Events.Interface;
using PaymentService.Events.Models;
using PaymentService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using PaymentService.DTOs;


namespace PaymentService.Events.RabbitMQ
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IPaymentService _paymentService;

        public RabbitMQConsumer(IPaymentService paymentService)
        {
            _paymentService = paymentService;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("order_exchange", ExchangeType.Direct, durable: true);
            _channel.QueueDeclare("payment_queue", durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind("payment_queue", "order_exchange", "order.created");
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
