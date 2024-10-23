using MessagingQueueLibrary.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MessagingQueueLibrary.Consumer
{
    public class RabbitMqConsumer : IMessageQueueConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqConsumer(RabbitMqConfig config)
        {
            var factory = new ConnectionFactory
            {
                HostName = config.Hostname,
                Port = config.Port,
                UserName = config.Username,
                Password = config.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public void Consume<T>(string topic, Func<T, Task> onMessageReceived)
        {
            string exchangeName = "ServerStatisticsExchange";
            string queueName = "ServerStatisticsQueue";

            _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            _channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false);
            _channel.QueueBind(queueName, exchangeName, topic);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var statistics = JsonSerializer.Deserialize<T>(message);

                await onMessageReceived?.Invoke(statistics);

                _channel.BasicAck(args.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queueName, autoAck: false, consumer);
        }

        ~RabbitMqConsumer()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
