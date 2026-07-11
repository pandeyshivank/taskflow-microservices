using TaskMicroService.RabbitMQ.Models;

namespace TaskMicroService.RabbitMQ.Interfaces
{
    public interface IMessagePublisher
    {
          public  Task PublishTaskCreatedAsync(TaskCreatedEventModel taskCreatedEventModel);
        
    }
}
