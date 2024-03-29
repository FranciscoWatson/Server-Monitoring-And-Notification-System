﻿using ServerStatisticsCollectionService.Messaging;
using ServerStatisticsCollectionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace ServerStatisticsCollectionService
{
    public class ServerStatisticsCollector
    {
        private readonly string _serverIdentifier;
        private readonly int _samplingIntervalSeconds;
        private readonly IMessageQueuePublisher _messageQueuePublisher;
        public ServerStatisticsCollector(string serverIdentifier, int samplingIntervalSeconds, IMessageQueuePublisher messageQueue)
        {
            _serverIdentifier = serverIdentifier;
            _samplingIntervalSeconds = samplingIntervalSeconds;
            _messageQueuePublisher = messageQueue;
        }

        public void StartAsync()
        {
            System.Timers.Timer timer = new(interval: _samplingIntervalSeconds * 1000);
            timer.Elapsed += async (sender, e) => await CollectAndPublishServerStatistics(null);
            timer.Start();
        }


        private Task CollectAndPublishServerStatistics(object state)
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

            _messageQueuePublisher.Publish($"ServerStatistics.{_serverIdentifier}", serverStatistics);
            return Task.CompletedTask;
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
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            return cpuCounter.NextValue();
        }
    }
    
}