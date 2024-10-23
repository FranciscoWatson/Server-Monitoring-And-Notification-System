using MessagingQueueLibrary.Models;
using MessagingQueueLibrary.Publisher;
using Microsoft.Extensions.Configuration;
using ServerStatisticsCollectionService.Models;
using ServerStatisticsCollectionService.Services;
using ServerStatisticsCollectionService.StatisticsCollectorStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerStatisticsCollectionService.Factories
{
    public class ServerStatisticsCollectorFactory
    {
        public ServerStatisticsCollector Create(IConfiguration configuration, IMessageQueuePublisher messageQueuePublisher)
        {
            var serverStatisticsConfig = configuration.GetSection("ServerStatisticsConfig").Get<ServerStatisticsConfig>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var strategy = new WindowsStatisticsCollectorStrategy();
                return new ServerStatisticsCollector(serverStatisticsConfig, messageQueuePublisher, strategy);
            }
            else
            {
                var strategy = new LinuxStatisticsCollectorStrategy();
                return new ServerStatisticsCollector(serverStatisticsConfig, messageQueuePublisher, strategy);
            }
        }
    }
}
