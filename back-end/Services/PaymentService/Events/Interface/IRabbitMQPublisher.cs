namespace PaymentService.Events.Interface
{
    public interface IRabbitMQPublisher
    {
        void Publish<T>(T message, string exchangeName, string routingKey);

    }
}
