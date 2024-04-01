using MessageProcessingService.Models;
using MessageProcessingService.Repository;
using MessageProcessingService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MessageProcessingService.Messaging
{
    public class RabbitMqConsumer : IMessageQueueConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServerStatisticsRepository _serverStatisticsRepository;
        private readonly AnomalyDetection _anomalyDetection;



        public RabbitMqConsumer(string hostname, int port, string username, string password, IServerStatisticsRepository serverStatisticsRepository, AnomalyDetection anomalyDetection)
        {
            var factory = new ConnectionFactory
            {
                HostName = hostname,
                Port = port,
                UserName = username,
                Password = password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _serverStatisticsRepository = serverStatisticsRepository;
            _anomalyDetection = anomalyDetection;
        }
        public void Consume(string topic)
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
                var statistics = JsonSerializer.Deserialize<ServerStatistics>(message);

                await _anomalyDetection.SendAnomalyAlertAsync(statistics);

                await _serverStatisticsRepository.InsertAsync(statistics);

                Console.WriteLine(message);


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
