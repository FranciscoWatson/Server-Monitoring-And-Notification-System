using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageProcessingService.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;


namespace MessageProcessingService.Services
{
    public class AnomalyDetection
    {
        private readonly HubConnection _hubConnection;
        private ServerStatistics _previousStatistics;
        private readonly double _memoryUsageAnomalyThresholdPercentage;
        private readonly double _cpuUsageAnomalyThresholdPercentage;
        private readonly double _memoryUsageThresholdPercentage;
        private readonly double _cpuUsageThresholdPercentage;
      

        public AnomalyDetection(SignalRConfig signalRConfig, AnomalyDetectionConfig anomalyDetectionConfig)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(signalRConfig.SignalRUrl)
                .Build();

            _memoryUsageAnomalyThresholdPercentage = anomalyDetectionConfig.MemoryUsageAnomalyThresholdPercentage;
            _cpuUsageAnomalyThresholdPercentage = anomalyDetectionConfig.CpuUsageAnomalyThresholdPercentage;
            _memoryUsageThresholdPercentage = anomalyDetectionConfig.MemoryUsageThresholdPercentage;
            _cpuUsageThresholdPercentage = anomalyDetectionConfig.CpuUsageThresholdPercentage;

            _previousStatistics = new ServerStatistics();
        }

        public async Task StartAsync()
        {
            await _hubConnection.StartAsync();
        }

        public async Task SendAnomalyAlertAsync(ServerStatistics currentStatistics)
        {



            if (currentStatistics.MemoryUsage > _previousStatistics.MemoryUsage * (1 + _memoryUsageAnomalyThresholdPercentage))
            {
                string alertMessage = $"Anomaly detected: Memory usage increased to {currentStatistics.MemoryUsage}MB, exceeding the threshold.";
                await _hubConnection.InvokeAsync("SendAlert", alertMessage);
                Console.WriteLine(alertMessage);
            }

            if (currentStatistics.CpuUsage > _previousStatistics.CpuUsage * (1 + _cpuUsageAnomalyThresholdPercentage))
            {
                string alertMessage = $"Anomaly detected: CPU usage increased to {currentStatistics.CpuUsage}%, exceeding the threshold.";
                await _hubConnection.InvokeAsync("SendAlert", alertMessage);
                Console.WriteLine(alertMessage);

            }

            if (currentStatistics.MemoryUsage / (currentStatistics.MemoryUsage + currentStatistics.AvailableMemory) > _memoryUsageThresholdPercentage)
            {
                string alertMessage = $"High usage detected: Memory usage is above the configured threshold.";
                await _hubConnection.InvokeAsync("SendAlert", alertMessage);
                Console.WriteLine(alertMessage);

            }

            if (currentStatistics.CpuUsage > _cpuUsageThresholdPercentage)
            {
                string alertMessage = $"High usage detected: CPU usage is above the configured threshold.";
                await _hubConnection.InvokeAsync("SendAlert", alertMessage);
                Console.WriteLine(alertMessage);
            }

            _previousStatistics = currentStatistics;
        }

    }
}
