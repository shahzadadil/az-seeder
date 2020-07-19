namespace Msa.Seeder.Azure.Interface
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

            foreach (var messageContent in messageContents)
            {
                var topicMessage = new Message(Encoding.UTF8.GetBytes(messageContent));
                await this._TopicClient.SendAsync(topicMessage);
            }
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