namespace Msa.Seeder.Steps.Azure.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Msa.Seeder.Core;

    public class PublishToTopicConfig : StepConfig
    {
        public PublishToTopicConfig(
            String connectionString,
            String topicName,
            IEnumerable<String> messageContents) : base(null)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (String.IsNullOrWhiteSpace(topicName))
            {
                throw new ArgumentNullException(nameof(topicName));
            }
            
            if (messageContents == null || !messageContents.Any())
            {
                throw new ArgumentNullException(nameof(messageContents));
            }

            this.ConnectionString = connectionString;
            this.TopicName = topicName;
            this.MessageContents = messageContents;
        }

        public String ConnectionString { get; private set; }
        public String TopicName { get; private set; }
        public IEnumerable<String> MessageContents { get; private set; }
    }
}