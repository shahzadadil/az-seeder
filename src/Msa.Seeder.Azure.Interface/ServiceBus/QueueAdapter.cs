namespace Msa.Seeder.Azure.Interface.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using global::Azure.Messaging.ServiceBus;
    using Msa.Seeder.Core.Configs;

    public class QueueAdapter
    {
        private readonly String _QueueName;
        private readonly String _ConnectionString;

        public QueueAdapter(String queueName, String connectionString)
        {
            if (String.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentNullException(nameof(queueName));
            }

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this._QueueName = queueName;
            this._ConnectionString = connectionString;
        }

        public async Task SendAtOnce(IEnumerable<DelayedContent<String>> messageContents)
        {
            if (messageContents == null)
            {
                throw new ArgumentNullException(nameof(messageContents));
            }

            if (!messageContents.Any())
            {
                return;
            }

            // var messageCount = messageContents.Count();

            // if (messageCount == 1)
            // {
            //     await SendMessage(messageContents.ElementAt(0));
            //     return;
            // } 

            // var lastMessage = messageContents.Last();
            // messageContents.ToList().RemoveAt(messageCount - 1);

            // Parallel.ForEach(
            //     messageContents,
            //     (msg) => SendMessage())

            await using (var queueClient = new ServiceBusClient(this._ConnectionString))
            {
                var messages = messageContents.Select(msg => new ServiceBusMessage(msg.Content));
                var sender = queueClient.CreateSender(this._QueueName);
                await sender.SendMessagesAsync(messages);
            }

            // foreach (var messageContent in messageContents)
            // {
            //     if (String.IsNullOrWhiteSpace(messageContent.Content))
            //     {
            //         throw new ArgumentException(nameof(messageContent.Content));
            //     }

            //     // Messages are sent synchronously by design, as we do not want to hold the processing
            //     var queueMessage = new Message(Encoding.UTF8.GetBytes(messageContent.Content));

            //     if (messageContent.Delay.TotalMilliseconds > 0)
            //     {
            //         var delay = Convert.ToInt32(Math.Floor(messageContent.Delay.TotalMilliseconds));
            //         Thread.Sleep(delay);
            //     }

            //     this._QueueClient.SendAsync(queueMessage);
            // }
        }        
    }    
}