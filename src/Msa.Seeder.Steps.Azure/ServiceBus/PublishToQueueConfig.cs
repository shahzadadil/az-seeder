namespace Msa.Seeder.Steps.Azure.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Msa.Seeder.Core;
    using Msa.Seeder.Core.Configs;

    public class PublishToQueueConfig : StepConfig
    {
        public PublishToQueueConfig(
            String connectionString,
            String queueName,
            IEnumerable<DelayedContent<String>> messageContents) : base(null)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (String.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentNullException(nameof(queueName));
            }
            
            if (messageContents == null || !messageContents.Any())
            {
                throw new ArgumentNullException(nameof(messageContents));
            }

            this.ConnectionString = connectionString;
            this.QueueName = queueName;
            this.MessageContents = messageContents;
        }

        public String ConnectionString { get; private set; }
        public String QueueName { get; private set; }
        public IEnumerable<DelayedContent<String>> MessageContents { get; private set; }
    }
}
