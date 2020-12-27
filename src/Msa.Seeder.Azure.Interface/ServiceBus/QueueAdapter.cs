namespace Msa.Seeder.Azure.Interface.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;
    using Msa.Seeder.Core.Configs;

    public class QueueAdapter : IDisposable
    {
        private readonly QueueClient _QueueClient;

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

            this._QueueClient = new QueueClient(connectionString, queueName);
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

            foreach (var messageContent in messageContents)
            {
                if (String.IsNullOrWhiteSpace(messageContent.Content))
                {
                    throw new ArgumentException(nameof(messageContent.Content));
                }

                // Messages are sent synchronously by design, as we do not want to hold the processing
                var queueMessage = new Message(Encoding.UTF8.GetBytes(messageContent.Content));

                if (messageContent.Delay.TotalMilliseconds > 0)
                {
                    var delay = Convert.ToInt32(Math.Floor(messageContent.Delay.TotalMilliseconds));
                    Thread.Sleep(delay);
                }

                this._QueueClient.SendAsync(queueMessage);
            }

            //await this._QueueClient.CloseAsync();
        }

        public void Dispose()
        {
            if (this._QueueClient != null)
            {
                //this._QueueClient.CloseAsync();
            }
        }
        
    }    
}