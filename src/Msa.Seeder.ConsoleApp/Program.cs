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
    using System.IO;

    public class Program
    {
        static async Task Main(string[] args)
        {
            var executor = new Executor();
            var assetOpsEvents = new List<AssetOperationalEvent>();
            var messages = new List<DelayedContent<String>>();
            var baseDate = new DateTime(2020, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            var random = new Random();

            for (int i = 0; i < 220; i++)
            {
                var assetOpsEvent = new AssetOperationalEvent
                {
                    AssetId = 10421,
                    Id = Guid.NewGuid(),
                    EventDateTime = baseDate.AddDays(i),
                    EventData = "TestData TestData TestData TestData TestData TestData TestData TestData TestData TestData",
                    IdentityId = random.Next(1, 10000),
                    Origin = "TEST",
                    Status = AssetOperationalEventStatus.InProgress
                };

                assetOpsEvents.Add(assetOpsEvent);
                messages.Add(
                    new DelayedContent<String>(
                        JsonConvert.SerializeObject(assetOpsEvent),
                        null
                    ));
            }

            var lastMessage = assetOpsEvents.Last();
            var rowKey = (DateTime.MaxValue.Ticks - lastMessage.EventDateTime.Ticks).ToString("d19");

            // executor
            //     .AddStep<PublishToTopicStep, PublishToTopicConfig>("Publish messages")
            //     .WithConfig(new PublishToTopicConfig(
            //         "",
            //         "",
            //         messages));

            // The JSON files need to be saved somewhere and path need to be provided here
            // It will build messages and send it to queue
            var files = Directory.EnumerateFiles(@"path to files");
            var queueMessages = files.Select(
                f => new DelayedContent<String>(
                    File.ReadAllText(f),
                    null)).ToList();

            // Specify the connection detaisl and queue name to which messages are to be sent
            // A delay can also be provided between messages if required by passing a delay
            // value above in the DelayedContent constructor
            executor
                .AddStep<PublishToQueueStep, PublishToQueueConfig>("Publish to Queue")
                .WithConfig(new PublishToQueueConfig(
                    "queue-conn-string", 
                    "queue-name",
                    queueMessages));

            // executor
            //     .AddStep<CheckRowInTableStep, CheckRowInTableConfig>("Check record in Table Storage")
            //     .WithConfig(new CheckRowInTableConfig(
            //         new TableStorageConfig(
            //             ""), 
            //         "",
            //         lastMessage.AssetId.ToString(), 
            //         rowKey,
            //         retryConfig: new RetryConfig(2000, 100)));

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
