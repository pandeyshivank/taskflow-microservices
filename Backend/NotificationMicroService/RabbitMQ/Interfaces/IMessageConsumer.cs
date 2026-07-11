namespace NotificationMicroService.RabbitMQ.Interfaces
{
    public interface IMessageConsumer
    {
        Task StartConsumingAsync(CancellationToken stoppingToken);

    }
}
