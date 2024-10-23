using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStatisticsCollectionService.StatisticsCollectorStrategies
{
    public class WindowsStatisticsCollectorStrategy : IStatisticsCollectorStrategy
    {
        public double GetMemoryUsage()
        {
            var memoryUsageCounter = new PerformanceCounter("Memory", "Committed Bytes");

            return memoryUsageCounter.NextValue() / 1048576;
        }

        public double GetAvailableMemory()
        {
            using var availableMemoryCounter = new PerformanceCounter("Memory", "Available MBytes");

            return availableMemoryCounter.NextValue();
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
