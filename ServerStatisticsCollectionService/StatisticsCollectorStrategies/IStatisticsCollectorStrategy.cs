using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStatisticsCollectionService.StatisticsCollectorStrategies
{
    public interface IStatisticsCollectorStrategy
    {
        double GetAvailableMemory();
        double GetCpuUsage();
        double GetMemoryUsage();
    }
}
