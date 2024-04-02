namespace MessagingQueueLibrary.Publisher
{
    public interface IMessageQueuePublisher
    {
        Task PublishAsync<T>(string topic, T message);
    }
}