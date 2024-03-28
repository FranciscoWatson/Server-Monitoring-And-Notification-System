using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStatisticsCollectionService.Messaging
{
    public interface IMessageQueuePublisher
    {
        void Publish<T>(string topic, T message);
    }
}
