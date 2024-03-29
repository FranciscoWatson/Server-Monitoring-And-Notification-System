using Microsoft.Extensions.Configuration;
using ServerStatisticsCollectionService.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStatisticsCollectionService
{
    public class MessageQueuePublisherFactory
    {
        public IMessageQueuePublisher Create(IConfiguration configuration)
        {
            var hostName = configuration.GetSection("RabbitMQConfig")["HostName"];
            var port = Convert.ToInt32(configuration.GetSection("RabbitMQConfig")["Port"]);
            var userName = configuration.GetSection("RabbitMQConfig")["UserName"];
            var password = configuration.GetSection("RabbitMQConfig")["Password"];

            return new RabbitMqPublisher(hostName, port, userName, password);
        }
    }
}
