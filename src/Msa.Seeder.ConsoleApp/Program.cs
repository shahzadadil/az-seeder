namespace Msa.Seeder.ConsoleApp
{
    using Msa.Seeder.Steps.Azure.ServiceBus;
    using Msa.Seeder.Engine;
    using Msa.Seeder.Steps.Azure.Storage;
    using Msa.Seeder.ConsoleApp.Data;
    using System;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Msa.Seeder.Core.Configs;
    using System.Threading.Tasks;
    using System.Linq;

    public class Program
    {
        static async Task Main(string[] args)
        {
            var executor = new Executor();
            var assetOpsEvents = new List<AssetOperationalEvent>();
            var messages = new List<String>();

            for (int i = 0; i < 1000; i++)
            {
                var assetOpsEvent = new AssetOperationalEvent
                {
                    AssetId = 111,
                    Id = Guid.NewGuid(),
                    EventDateTime = DateTime.UtcNow,
                    EventData = "TestData",
                    IdentityId = 1,
                    Origin = "TEST",
                    Status = AssetOperationalEventStatus.InProgress
                };

                assetOpsEvents.Add(assetOpsEvent);
                messages.Add(JsonConvert.SerializeObject(assetOpsEvent));
            }

            var lastMessage = assetOpsEvents.Last();
            var rowKey = (DateTime.MaxValue.Ticks - lastMessage.EventDateTime.Ticks).ToString("d19");

            executor
                .AddStep<PublishToTopicStep, PublishToTopicConfig>("Publish messages")
                .WithConfig(new PublishToTopicConfig(
                    "Endpoint=sb://jl-assetops-test.servicebus.windows.net/;SharedAccessKeyName=writeonly;SharedAccessKey=ch+feapaI1jMmKrkMRaRfQI9HlL3RG+e4cTWg4+PjHM=", 
                    "assetopsevents",
                    messages));

            executor
                .AddStep<CheckRowInTableStep, CheckRowInTableConfig>("Check record in Table Storage")
                .WithConfig(new CheckRowInTableConfig(
                    new TableStorageConfig(
                        "DefaultEndpointsProtocol=https;AccountName=assetopsstoragetest;AccountKey=pc2mTiRFEUJlqar/uOPEDPOJ1K0MTDZCQBllxSiGuHIvphRQuox9s7s9LUHpx7z/gjs6hr8WvcJvvQDqTR7UaQ==;EndpointSuffix=core.windows.net"), 
                    "assetopsevents",
                    lastMessage.AssetId.ToString(), 
                    rowKey,
                    retryConfig: new RetryConfig(1, 100)));

            var executionMetric = await executor.Execute();

            if (!executionMetric.IsExecutionSuccessful)
            {
                var errorStep = executionMetric.Last();
                Console.WriteLine($"Error executing steps. {errorStep}");
                Console.WriteLine(executionMetric);

                return;
            }

            var publishStepMetric = executionMetric[0];
            var verificationStepMetric = executionMetric[1];

            var start = publishStepMetric.Duration.Start.Value;
            var end = verificationStepMetric.Duration.End.Value;
            var executionTimespan = end.Subtract(start);

            Console.WriteLine(executionMetric);
        }
    }
}
