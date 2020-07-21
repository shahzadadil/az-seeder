namespace Msa.Seeder.ConsoleApp
{
    using Msa.Seeder.Steps.Azure.ServiceBus;
    using Msa.Seeder.Engine;
    using Msa.Seeder.Steps.Azure.Storage;

    public class Program
    {
        static void Main(string[] args)
        {
            var executor = new Executor();

            executor
                .AddStep<PublishToTopicStep, PublishToTopicConfig>("Publish messages")
                .WithConfig(new PublishToTopicConfig(null, null, null));

            executor
                .AddStep<CheckRowInTableStep, CheckRowInTableConfig>("Check record in Table Storage")
                .WithConfig(new CheckRowInTableConfig(null, null, null, null));

            executor.Execute();
        }
    }
}
