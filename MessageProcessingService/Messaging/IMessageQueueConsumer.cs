
namespace MessageProcessingService.Messaging
{
    public interface IMessageQueueConsumer
    {
        void Consume<T>(string topic, Func<T, Task> onMessageReceived);
    }
}
