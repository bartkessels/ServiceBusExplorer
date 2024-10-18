namespace ServiceBusExplorer
{
    public class MessagingFactory
    {
        public QueueClient CreateQueueClient()
        {
            return new QueueClient();
        }
    }
}
