using Azure.Messaging.ServiceBus;
using Serilog;
using System;
using System.Threading.Tasks;

namespace RecipientList.Services
{
    internal class MessageConstruction : IMessageConstruction
    {
        private static readonly string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        private static readonly string topicName = Environment.GetEnvironmentVariable("topic");

        private static ServiceBusClient client;
        private static ServiceBusSender sender;

        public async Task CreateSendMessage(MessageTransform m)
        {
            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(topicName);

            // Clear existing Queueu
            ClearServiceBus().Wait();

            // Execute 1000000 messages
            int i = 0;
            do
            {
                ServiceBusMessage message = new ServiceBusMessage()
                {
                    MessageId = Guid.NewGuid().ToString(),
                    CorrelationId = $"OrderApp-{Guid.NewGuid()}",
                    ContentType = "application/json",
                    Subject = $"Order id: {i}",
                    ReplyTo = "orderapp-100",
                    To = "InventoryControl",
                    Body = BinaryData.FromString($"{m.MessageTransformation()}")
                };

                try
                {
                    await sender.SendMessageAsync(message);
                }
                catch (Exception e)
                {
                    Log.Information(e.ToString());
                }

                i++;
            } while (i < 1000000);

            // Close connection
            await sender.DisposeAsync();
            await client.DisposeAsync();
        }

        public static async Task ClearServiceBus()
        {
            ServiceBusReceiver receiver = client.CreateReceiver(topicName, new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

            while ((await receiver.PeekMessageAsync()) != null)
            {
                // receive in batches of 100 messages.
                await receiver.ReceiveMessagesAsync(100);
            }
        }
    }
}