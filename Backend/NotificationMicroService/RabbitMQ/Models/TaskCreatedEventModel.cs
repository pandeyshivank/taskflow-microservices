namespace NotificationMicroService.RabbitMQ.Models
{
    public class TaskCreatedEventModel
    {
        public Guid TaskId { get; set; }

        public Guid UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
