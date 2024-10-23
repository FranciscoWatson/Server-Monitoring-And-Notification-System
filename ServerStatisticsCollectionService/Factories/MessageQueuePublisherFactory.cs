using MessagingQueueLibrary;
using MessagingQueueLibrary.Models;
using MessagingQueueLibrary.Publisher;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStatisticsCollectionService.Factories
{
    public class MessageQueuePublisherFactory
    {
        public IMessageQueuePublisher Create(IConfiguration configuration)
        {
            var rabbitMQConfig = configuration.GetSection("RabbitMQConfig").Get<RabbitMqConfig>();

            return new RabbitMqPublisher(rabbitMQConfig);
        }
    }
}
