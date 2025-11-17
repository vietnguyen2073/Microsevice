namespace PaymentService.Events.Interface
{
    public interface IRabbitMQConsumer
    {
        void StartConsuming();
    }
}
