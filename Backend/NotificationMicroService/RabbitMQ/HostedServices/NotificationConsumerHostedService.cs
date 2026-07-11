using NotificationMicroService.RabbitMQ.Interfaces;

namespace NotificationMicroService.RabbitMQ.HostedServices
{
    public class NotificationConsumerHostedService : BackgroundService
    {
        private readonly IMessageConsumer _consumer;
        public NotificationConsumerHostedService(IMessageConsumer consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.StartConsumingAsync(stoppingToken);
        }
    }

    
}
