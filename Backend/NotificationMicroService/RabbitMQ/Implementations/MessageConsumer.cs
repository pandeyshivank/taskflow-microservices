using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel;
using Microsoft.Extensions.Options;
using NotificationMicroService.RabbitMQ.Interfaces;
using NotificationMicroService.RabbitMQ.Settings;
using RabbitMQ.Client;
using NotificationMicroService.RabbitMQ.Models;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NotificationMicroService.Repositories.Interfaces;
using NotificationMicroService.Entities;
using System.Threading;
using System.Runtime.Intrinsics.Arm;

namespace NotificationMicroService.RabbitMQ.Implementations
{
    public class MessageConsumer : IMessageConsumer
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly RabbitMQSettings _settings;
        private readonly IServiceScopeFactory _scopeFactory;
        public MessageConsumer (IOptions<RabbitMQSettings> options, IServiceScopeFactory scopeFactory)
        {

            _settings = options.Value;
            _scopeFactory = scopeFactory;
        }

        public async Task StartConsumingAsync(CancellationToken stoppingToken)
        
        {
            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            // connection created now service connected to rabbit MQ
             _connection = await factory.CreateConnectionAsync();

            // channel created as we know we can create multiple channel inside connection
             _channel = await _connection.CreateChannelAsync();

            // as we know we need exchnage to send message to queue,(publiser directly not sent message to queue its goes through exchange
            await _channel.ExchangeDeclareAsync(
            exchange: _settings.ExchangeName,
            type: ExchangeType.Direct,
            durable: true);

            // as we know we need queue to store  message which come from exchange to queue,(publiser directly not sent message to queue its goes through exchange
            await _channel.QueueDeclareAsync(
             queue: _settings.QueueName,
             durable: true,
             exclusive: false,
             autoDelete: false);

            await _channel.QueueBindAsync(
               queue: _settings.QueueName,
               exchange: _settings.ExchangeName,
               routingKey: _settings.RoutingKey);



            var consumer = new AsyncEventingBasicConsumer(_channel);

            // now from this consumer we will create event receive messages or RabbitMQ is event-driven.RabbitMQ itself calls this method whenever a new message arrives. when new
            // new message come it Received Event Fires Automatically.
            consumer.ReceivedAsync += async (sender, args) =>
            {
                try
                {
                    string json = Encoding.UTF8.GetString(args.Body.ToArray());
                    TaskCreatedEventModel? task = JsonSerializer.Deserialize<TaskCreatedEventModel>(json);
                    if (task == null)
                    {
                        return;
                    }
                    using var scope = _scopeFactory.CreateScope();

                    // creating respositry object or refrence with help of service factory so get scoped ovject inside this singlton method
                    var repository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();



                    var notification = new Entities.Notification()
                    {
                        CreatedAt = DateTime.UtcNow,
                        UserId = task.UserId,
                        Title = task.Title,
                        Message = task.Description,
                        NotificationType = "New",
                        IsRead = false,

                    };
                    // saving receiver notification 
                    await repository.CreateNotificationAsync(notification);

                    // Only after successful SQL save.  This tells RabbitMQ Message Successfully Processed and Delete From Queue
                    await _channel.BasicAckAsync(args.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    // We'll add logging here later
                    
                    Console.WriteLine(ex.Message);
                    // Don't ACK.
                    // RabbitMQ will deliver the message again.
                }

            };

            // again check teh queue and Start Listening again for new message so checking the queue after finishing previous message 
            await _channel.BasicConsumeAsync(
               queue: _settings.QueueName,
               autoAck: false,
               consumer: consumer);
            // HostedService finishes consumer stop so keep running we will put in infinite loop so keep looking 
            await Task.Delay(
              Timeout.Infinite,
              stoppingToken);



        }

    }
}
