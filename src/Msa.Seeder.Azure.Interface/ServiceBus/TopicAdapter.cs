namespace Msa.Seeder.Azure.Interface.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using global::Azure.Messaging.ServiceBus;
    using Msa.Seeder.Core.Configs;

    public class TopicAdapter
    {
        private readonly ServiceBusClient topicClient;
        private readonly String _ConnectionString;
        private readonly String _TopicName;

        public TopicAdapter(String topicName, String connectionString)
        {
            if (String.IsNullOrWhiteSpace(topicName))
            {
                throw new ArgumentNullException(nameof(topicName));
            }

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this._ConnectionString = connectionString;
            this._TopicName = topicName;
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

            await using (var topicClient = new ServiceBusClient(this._ConnectionString))
            {
                var sender = this.topicClient.CreateSender(this._TopicName);
                var topicMessages = messageContents.Select(msg => new ServiceBusMessage(msg.Content));
                await sender.SendMessagesAsync(topicMessages);
            }
            

            // foreach (var messageContent in messageContents)
            // {
            //     if (String.IsNullOrWhiteSpace(messageContent.Content))
            //     {
            //         throw new ArgumentException(nameof(messageContent.Content));
            //     }

            //     // Messages are sent synchronously by design, as we do not want to hold the processing
            //     // var topicMessage = new Message(Encoding.UTF8.GetBytes(messageContent.Content));
            //     var topicMessage = new Message(Encoding.UTF8.GetBytes("test"));

            //     // if (messageContent.Delay.TotalMilliseconds > 0)
            //     // {
            //     //     var delay = Convert.ToInt32(Math.Floor(messageContent.Delay.TotalMilliseconds));
            //     //     Thread.Sleep(delay);
            //     // }

            //     sender.SendMessagesAsync
            //     //await client.CloseAsync();
            // }
        }        
    }    
}