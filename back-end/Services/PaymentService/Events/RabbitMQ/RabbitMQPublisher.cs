using Newtonsoft.Json;
using PaymentService.Events.Interface;
using RabbitMQ.Client;
using System.Text;
//using Microsoft.EntityFrameworkCore.Metadata;

namespace OrderService.Events.RabbitMQ
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQPublisher()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Publish<T>(T message, string exchangeName, string routingKey) 
        {
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: exchangeName,
                routingKey: routingKey,
                basicProperties: null,
                body: body
            );
        }

    }
}
