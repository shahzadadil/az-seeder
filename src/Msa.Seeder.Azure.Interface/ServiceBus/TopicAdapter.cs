namespace Msa.Seeder.Azure.Interface.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;

    public class TopicAdapter : IDisposable
    {
        private readonly TopicClient _TopicClient;

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

            this._TopicClient = new TopicClient(connectionString, topicName);
        }        

        public async Task SendAtOnce(IEnumerable<String> messageContents)
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
                var topicMessage = new Message(Encoding.UTF8.GetBytes(messageContent));
                this._TopicClient.SendAsync(topicMessage);
            }
        }

        private async Task SendMessage(String messageContent)
        {
            var topicMessage = new Message(Encoding.UTF8.GetBytes(messageContent));
            await this._TopicClient.SendAsync(topicMessage);
        }

        public void Dispose()
        {
            if (this._TopicClient != null)
            {
                this._TopicClient.CloseAsync();
            }
        }
        
    }    
}