using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingService.Models
{
    public class AnomalyDetectionConfig
    {
        public double MemoryUsageAnomalyThresholdPercentage { get; set; }
        public double CpuUsageAnomalyThresholdPercentage { get; set; }
        public double MemoryUsageThresholdPercentage { get; set; }
        public double CpuUsageThresholdPercentage { get; set; }
    }
}
