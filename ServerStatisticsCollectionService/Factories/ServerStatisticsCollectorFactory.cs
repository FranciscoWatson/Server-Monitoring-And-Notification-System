using MessagingQueueLibrary.Models;
using MessagingQueueLibrary.Publisher;
using Microsoft.Extensions.Configuration;
using ServerStatisticsCollectionService.Models;
using ServerStatisticsCollectionService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStatisticsCollectionService.Factories
{
    public class ServerStatisticsCollectorFactory
    {
        public ServerStatisticsCollector Create(IConfiguration configuration, IMessageQueuePublisher messageQueuePublisher)
        {
            var serverStatisticsConfig = configuration.GetSection("ServerStatisticsConfig").Get<ServerStatisticsConfig>();

            return new ServerStatisticsCollector(serverStatisticsConfig, messageQueuePublisher);
        }
    }
}
