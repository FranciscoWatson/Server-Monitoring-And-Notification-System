using ServerStatisticsCollectionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MessagingQueueLibrary.Publisher;
using ServerStatisticsCollectionService.StatisticsCollectorStrategies;


namespace ServerStatisticsCollectionService.Services
{
    public class ServerStatisticsCollector
    {
        private readonly string _serverIdentifier;
        private readonly int _samplingIntervalSeconds;
        private readonly IMessageQueuePublisher _messageQueuePublisher;
        private readonly IStatisticsCollectorStrategy _statisticsCollectorStrategy;
        public ServerStatisticsCollector(ServerStatisticsConfig config, IMessageQueuePublisher messageQueue, IStatisticsCollectorStrategy statisticsCollectorStrategy)
        {
            _serverIdentifier = config.ServerIdentifier;
            _samplingIntervalSeconds = config.SamplingIntervalSeconds;
            _messageQueuePublisher = messageQueue;
            _statisticsCollectorStrategy = statisticsCollectorStrategy;
        }

        public void StartAsync()
        {
            System.Timers.Timer timer = new(interval: _samplingIntervalSeconds * 1000);
            timer.Elapsed += async (sender, e) => await CollectAndPublishServerStatistics(null);
            timer.Start();
        }


        private async Task CollectAndPublishServerStatistics(object state)
        {
            double memoryUsage = _statisticsCollectorStrategy.GetMemoryUsage();
            double availableMemory = _statisticsCollectorStrategy.GetAvailableMemory();
            double cpuUsage = _statisticsCollectorStrategy.GetCpuUsage();

            var serverStatistics = new ServerStatistics
            {
                MemoryUsage = memoryUsage,
                AvailableMemory = availableMemory,
                CpuUsage = cpuUsage,
                Timestamp = DateTime.Now
            };

            await _messageQueuePublisher.PublishAsync($"ServerStatistics.{_serverIdentifier}", serverStatistics);
        }

        

    }

}
