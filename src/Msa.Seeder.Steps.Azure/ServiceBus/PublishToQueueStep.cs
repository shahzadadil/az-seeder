namespace Msa.Seeder.Steps.Azure.ServiceBus
{
    using System;
    using System.Threading.Tasks;
    using Msa.Seeder.Azure.Interface.ServiceBus;
    using Msa.Seeder.Core;

    public class PublishToQueueStep : Step<PublishToQueueConfig>
    {
        public override async Task Execute()
        {
            var stepConfig = this._StepConfig;

            using (var queueAdapter = new QueueAdapter(stepConfig.QueueName, stepConfig.ConnectionString))
            {
                await queueAdapter.SendAtOnce(stepConfig.MessageContents);
            }
        }
    }
}