namespace Msa.Seeder.Steps.Azure.ServiceBus
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Msa.Seeder.Azure.Interface.ServiceBus;
    using Msa.Seeder.Core;

    public class PublishToTopicStep : Step<PublishToTopicConfig>
    {
        public override async Task Execute()
        {
            var stepConfig = this._StepConfig;

            // using (var topicAdapter = new TopicAdapter(stepConfig.TopicName, stepConfig.ConnectionString))
            // {
                var topicAdapter = new TopicAdapter(stepConfig.TopicName, stepConfig.ConnectionString);
                await topicAdapter.SendAtOnce(stepConfig.MessageContents.ToList());
            //}
        }
    }
}