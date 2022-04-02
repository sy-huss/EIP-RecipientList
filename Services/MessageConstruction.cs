using Azure.Messaging.ServiceBus;
using Serilog;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CompetingConsumer.Services
{
    internal class MessageConstruction : IMessageConstruction
    {
        private static string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        private static string queueName = Environment.GetEnvironmentVariable("queue");

        private static ServiceBusClient client;
        private static ServiceBusSender sender;

        public async Task CreateSendMessage(MessageTransform m)
        {
            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(queueName);

            // Clear existing Queueu
            ClearServiceBus().Wait();

            // Execute 1000000 messages
            int i = 0;
            do
            {
                ServiceBusMessage message = new ServiceBusMessage()
                {
                    MessageId = Guid.NewGuid().ToString(), // Unique ide for this message
                    CorrelationId = $"OrderApp-{Guid.NewGuid()}", // ID used to correlat the message back to the sender if required
                    ContentType = "application/json",
                    Subject = $"Order id: {i}",
                    ReplyTo = "orderapp-100",
                    Body = BinaryData.FromString($"{m.MessageTransformation()}")
                };

                try
                {
                    await sender.SendMessageAsync(message);
                }
                catch (Exception e)
                {
                }

                i++;
            } while (i < 1000000);

            // Close connection
            await sender.DisposeAsync();
            await client.DisposeAsync();
        }

        public static async Task ClearServiceBus()
        {
            ServiceBusReceiver receiver = client.CreateReceiver(queueName, new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

            while ((await receiver.PeekMessageAsync()) != null)
            {
                // receive in batches of 100 messages.
                await receiver.ReceiveMessagesAsync(100);
            }
        }
    }
}