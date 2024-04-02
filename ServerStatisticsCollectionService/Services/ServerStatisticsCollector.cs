using ServerStatisticsCollectionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MessagingQueueLibrary.Publisher;


namespace ServerStatisticsCollectionService.Services
{
    public class ServerStatisticsCollector
    {
        private readonly string _serverIdentifier;
        private readonly int _samplingIntervalSeconds;
        private readonly IMessageQueuePublisher _messageQueuePublisher;
        public ServerStatisticsCollector(ServerStatisticsConfig config, IMessageQueuePublisher messageQueue)
        {
            _serverIdentifier = config.ServerIdentifier;
            _samplingIntervalSeconds = config.SamplingIntervalSeconds;
            _messageQueuePublisher = messageQueue;
        }

        public void StartAsync()
        {
            System.Timers.Timer timer = new(interval: _samplingIntervalSeconds * 1000);
            timer.Elapsed += async (sender, e) => await CollectAndPublishServerStatistics(null);
            timer.Start();
        }


        private async Task CollectAndPublishServerStatistics(object state)
        {
            double memoryUsage = GetMemoryUsage();
            double availableMemory = GetAvailableMemory();
            double cpuUsage = GetCpuUsage();

            var serverStatistics = new ServerStatistics
            {
                MemoryUsage = memoryUsage,
                AvailableMemory = availableMemory,
                CpuUsage = cpuUsage,
                Timestamp = DateTime.Now
            };

            await _messageQueuePublisher.PublishAsync($"ServerStatistics.{_serverIdentifier}", serverStatistics);
        }

        private double GetMemoryUsage()
        {
            return GetTotalMemory() - GetAvailableMemory();
        }

        private double GetTotalMemory()
        {
            var gcMemoryInfo = GC.GetGCMemoryInfo();
            var installedMemory = gcMemoryInfo.TotalAvailableMemoryBytes / (1024.0 * 1024.0);
            return installedMemory;

        }

        private double GetAvailableMemory()
        {
            var ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            return ramCounter.NextValue();

        }

        public double GetCpuUsage()
        {
            using (var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
            {
                cpuCounter.NextValue();
                Thread.Sleep(1000);
                return cpuCounter.NextValue();
            }
        }

    }

}
