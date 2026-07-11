namespace NotificationMicroService.RabbitMQ.Settings
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ExchangeName { get; set; } = string.Empty;

        public string QueueName { get; set; } = string.Empty;

        public string RoutingKey { get; set; } = string.Empty;
    }
}
