using MessagingQueueLibrary.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MessagingQueueLibrary.Publisher
{
    public class RabbitMqPublisher : IMessageQueuePublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;


        public RabbitMqPublisher(RabbitMqConfig config)
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

        public async Task PublishAsync<T>(string topic, T message)
        {
            string exchangeName = "ServerStatisticsExchange";
            string queueName = "ServerStatisticsQueue";

            _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            _channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false);
            _channel.QueueBind(queueName, exchangeName, topic);


            string jsonMessage = JsonSerializer.Serialize(message);
            var bytesMessage = Encoding.UTF8.GetBytes(jsonMessage);


            await Task.Run(() => _channel.BasicPublish(
                exchange: exchangeName,
                routingKey: topic,
                basicProperties: null,
                body: bytesMessage
            ));

        }

        ~RabbitMqPublisher()
        {
            _channel?.Close();
            _connection?.Close();
        }

    }
}
