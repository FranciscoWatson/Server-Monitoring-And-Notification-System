using Microsoft.Extensions.Configuration;
using ServerStatisticsCollectionService.Messaging;
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
            var samplingIntervalSeconds = Convert.ToInt32(configuration.GetSection("ServerStatisticsConfig")["SamplingIntervalSeconds"]);
            var serverIdentifier = configuration.GetSection("ServerStatisticsConfig")["ServerIdentifier"];

            return new ServerStatisticsCollector(serverIdentifier, samplingIntervalSeconds, messageQueuePublisher);
        }
    }
}
