using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingQueueLibrary.Consumer
{
    public interface IMessageQueueConsumer
    {
        void Consume<T>(string topic, Func<T, Task> onMessageReceived);
    }
}
