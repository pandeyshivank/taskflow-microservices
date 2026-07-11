using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TaskMicroService.RabbitMQ.Interfaces;
using TaskMicroService.RabbitMQ.Models;
using TaskMicroService.RabbitMQ.Settings;

namespace TaskMicroService.RabbitMQ.Implementations
{
    public class RabbitMQMessagePublisher : IMessagePublisher
    {
        
        private readonly RabbitMQSettings _settings;
        public RabbitMQMessagePublisher(IOptions<RabbitMQSettings> options)
        {

            _settings = options.Value;
        }
        public async Task PublishTaskCreatedAsync(TaskCreatedEventModel taskCreatedEventModel)
        {
            //estable a connection from rabbit mq server its a tcp connection.
            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            // connection created now service connected to rabbit MQ
            using var connection = await factory.CreateConnectionAsync();

            // channel created as we know we can create multiple channel inside connection 
            using var channel = await connection.CreateChannelAsync();

            // as we know we need exchnage to send message to queue,(publiser directly not sent message to queue its goes through exchange
            await channel.ExchangeDeclareAsync(
            exchange: _settings.ExchangeName,
            type: ExchangeType.Direct,
            durable: true);

            // as we know we need queue to store  message which come from exchange to queue,(publiser directly not sent message to queue its goes through exchange
            await channel.QueueDeclareAsync(
             queue: _settings.QueueName,
             durable: true,
             exclusive: false,
             autoDelete: false);

            // As we know exchange and queue has binding so it know from exchnage to which queue msg need to send that binding via routh key like address. because mutiple queue can present
             await channel.QueueBindAsync(
                queue: _settings.QueueName,
                exchange: _settings.ExchangeName,
                routingKey: _settings.RoutingKey);

            //data need to serialize which need to send and convert into byte array to secure transmission.

            string json = JsonSerializer.Serialize(taskCreatedEventModel);

            byte[] body = Encoding.UTF8.GetBytes(json);


            // publish the message finally 
            await channel.BasicPublishAsync(
            exchange: _settings.ExchangeName,
            routingKey: _settings.RoutingKey,
            body: body);
        }
    }
}
