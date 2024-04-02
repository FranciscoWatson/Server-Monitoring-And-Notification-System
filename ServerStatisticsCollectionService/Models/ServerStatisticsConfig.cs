using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStatisticsCollectionService.Models
{
    public class ServerStatisticsConfig
    {
        public int SamplingIntervalSeconds { get; set; }
        public string ServerIdentifier { get; set; }
    }
}
