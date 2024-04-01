using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageProcessingService.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;


namespace MessageProcessingService
{
    public class AnomalyDetection
    {
        private readonly HubConnection _hubConnection;
        private readonly IConfiguration _configuration;
        private ServerStatistics _previousStatistics;


        public AnomalyDetection(string signalRHubUrl, IConfiguration configuration)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(signalRHubUrl)
                .Build();

            
            _configuration = configuration;
            _previousStatistics = new ServerStatistics();
        }

        public async Task StartAsync()
        {
            await _hubConnection.StartAsync();
        }

        public async Task SendAnomalyAlertAsync(ServerStatistics currentStatistics)
        {


            var memoryUsageAnomalyThresholdPercentage = Convert.ToDouble(_configuration.GetSection("AnomalyDetectionConfig")["MemoryUsageAnomalyThresholdPercentage"]);
            var cpuUsageAnomalyThresholdPercentage = Convert.ToDouble(_configuration.GetSection("AnomalyDetectionConfig")["CpuUsageAnomalyThresholdPercentage"]);
            var memoryUsageThresholdPercentage = Convert.ToDouble(_configuration.GetSection("AnomalyDetectionConfig")["MemoryUsageThresholdPercentage"]);
            var cpuUsageThresholdPercentage = Convert.ToDouble(_configuration.GetSection("AnomalyDetectionConfig")["CpuUsageThresholdPercentage"]);

            if (currentStatistics.MemoryUsage > (_previousStatistics.MemoryUsage * (1 + memoryUsageAnomalyThresholdPercentage)))
            {
                string alertMessage = $"Anomaly detected: Memory usage increased to {currentStatistics.MemoryUsage}MB, exceeding the threshold.";
                await _hubConnection.InvokeAsync("SendAlert", alertMessage);
                Console.WriteLine(alertMessage);
            }

            if (currentStatistics.CpuUsage > (_previousStatistics.CpuUsage * (1 + cpuUsageAnomalyThresholdPercentage)))
            {
                string alertMessage = $"Anomaly detected: CPU usage increased to {currentStatistics.CpuUsage}%, exceeding the threshold.";
                await _hubConnection.InvokeAsync("SendAlert", alertMessage);
                Console.WriteLine(alertMessage);

            }

            if ((currentStatistics.MemoryUsage / (currentStatistics.MemoryUsage + currentStatistics.AvailableMemory)) > memoryUsageThresholdPercentage)
            {
                string alertMessage = $"High usage detected: Memory usage is above the configured threshold.";
                await _hubConnection.InvokeAsync("SendAlert", alertMessage);
                Console.WriteLine(alertMessage);

            }

            if (currentStatistics.CpuUsage > cpuUsageThresholdPercentage)
            {
                string alertMessage = $"High usage detected: CPU usage is above the configured threshold.";
                await _hubConnection.InvokeAsync("SendAlert", alertMessage);
                Console.WriteLine(alertMessage);
            }

            _previousStatistics = currentStatistics;
        }

    }
}
