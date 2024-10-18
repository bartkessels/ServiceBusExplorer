using Azure.Core;
using Azure.Messaging.ServiceBus;

namespace ServiceBusExplorer
{
    public class MessagingFactory
    {
        private readonly ServiceBusClient _client;

        private MessagingFactory(string fullyQualifiedNamespace, TokenCredential tokenCredential)
        {
            _client = new ServiceBusClient(fullyQualifiedNamespace, tokenCredential);
        }

        private MessagingFactory(string connectionString)
        {
            _client = new ServiceBusClient(connectionString);
        }

        public static MessagingFactory CreateFromConnectionString(string connectionString)
        {
            return new MessagingFactory(connectionString);
        }

        public static MessagingFactory CreateFromTokenCredential(string fullyQualifiedNamespace, TokenCredential tokenCredential)
        {
            return new MessagingFactory(fullyQualifiedNamespace, tokenCredential);
        }

        public QueueClient CreateQueueClient(string path, ServiceBusReceiveMode receiveMode)
        {
            return new QueueClient(_client, path, receiveMode);
        }

        public ServiceBusReceiver CreateMessageReceiver(string path, ServiceBusReceiveMode receiveMode)
        {
            return _client.CreateReceiver(path, new ServiceBusReceiverOptions
            {
                ReceiveMode = receiveMode
            });
        }
    }
}
