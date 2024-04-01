using MessageProcessingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingService.Repository
{
    public interface IServerStatisticsRepository
    {
        Task InsertAsync(ServerStatistics serverStatistics);
    }
}
