using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStatisticsCollectionService.Messaging
{
    public interface IMessageQueuePublisher
    {
        Task PublishAsync<T>(string topic, T message);
    }
}
