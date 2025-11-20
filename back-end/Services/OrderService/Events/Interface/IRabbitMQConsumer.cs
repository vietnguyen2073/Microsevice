namespace OrderService.Events.Interface
{
    public interface IRabbitMQConsumer
    {
        void StartConsuming();
    }
}
