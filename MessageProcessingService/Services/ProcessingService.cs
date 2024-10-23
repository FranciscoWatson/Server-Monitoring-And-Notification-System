using MessageProcessingService.Models;
using MessageProcessingService.Repository;
using MessagingQueueLibrary.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingService.Services
{
    public class ProcessingService
    {
        private readonly IServerStatisticsRepository _serverStatisticsRepository;
        private readonly AnomalyDetection _anomalyDetection;
        private readonly IMessageQueueConsumer _messageQueueConsumer;

        public ProcessingService(IServerStatisticsRepository serverStatisticsRepository, AnomalyDetection anomalyDetection, IMessageQueueConsumer messageQueueConsumer)
        {
            _serverStatisticsRepository = serverStatisticsRepository;
            _anomalyDetection = anomalyDetection;
            _messageQueueConsumer = messageQueueConsumer;
        }

        public async Task ProcessMessageAsync()
        {
            await Task.Run(() => _messageQueueConsumer.Consume<ServerStatistics>("ServerStatistics.*", async statistics =>
            {
                await HandleServerStatistics(statistics);
            }));
        }

        private async Task HandleServerStatistics(ServerStatistics statistics)
        {
            await _anomalyDetection.SendAnomalyAlertAsync(statistics);
            await _serverStatisticsRepository.InsertAsync(statistics);
        }
    }
}
