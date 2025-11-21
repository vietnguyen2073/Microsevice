using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace OrderService.BackgroundServices
{

    public abstract class RabbitMQListenService<T> : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        protected readonly IServiceProvider _serviceProvider;
        private readonly string _queueName;
        private readonly string _exchangeName;
        private readonly string _routingKey;

        protected RabbitMQListenService(
            IServiceProvider serviceProvider,
            string queueName,
            string exchangeName,
            string routingKey)
        {
            _serviceProvider = serviceProvider;
            _queueName = queueName;
            _exchangeName = exchangeName;
            _routingKey = routingKey;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, durable: true);
            _channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(_queueName, _exchangeName, _routingKey);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await HandleMessage(message);
                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(_queueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        protected abstract Task HandleMessage(string rawMessage);
    }
}
