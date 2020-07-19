namespace Msa.Seeder.ConsoleApp
{
    using Msa.Seeder.Steps.Azure.ServiceBus;
    using Msa.Seeder.Engine;

    public class Program
    {
        static void Main(string[] args)
        {
            var executor = new Executor();

            executor
                .AddStep<PublishToTopicStep, PublishToTopicConfig>("Publish messages")
                .WithConfig(new PublishToTopicConfig(null, null, null));

            executor.Execute();

        }
    }
}
