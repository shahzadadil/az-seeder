using System.Threading.Tasks;
using Msa.Seeder.Azure.Interface;

namespace Msa.Seeder.Azure.Steps.ServiceBus
{
    public class PublishToTopicStep : Step<PublishToTopicConfig>
    {
        public PublishToTopicStep()
        {
        }

        public override async Task Execute()
        {
            using (var topicAdapter = new TopicAdapter(this._StepConfig.TopicName, this._StepConfig.ConnectionString))
            {
                await topicAdapter.SendAtOnce(this._StepConfig.MessageContents);
            }            
        }
    }
}