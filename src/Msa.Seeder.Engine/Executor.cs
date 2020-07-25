namespace Msa.Seeder.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Msa.Seeder.Core;
    using Msa.Seeder.Core.Metrics;
    using Msa.Core;
    using Msa.Core.Infra;

    public class Executor
    {
        private ICollection<IStep> _Steps;

        public Executor()
        {
            this._Steps = new List<IStep>();
        }

        public TStep AddStep<TStep, TConfig>(String stepName) 
            where TStep: Step<TConfig>
            where TConfig: StepConfig
        {
            if (String.IsNullOrWhiteSpace(stepName))
            {
                throw new ArgumentNullException(nameof(stepName));
            }

            var step = (TStep)Activator.CreateInstance(typeof(TStep));
            step.Create(stepName, this._Steps.Count + 1);
            _Steps.Add(step);

            return step;
        }

        public async Task<ExecutionMetric> Execute()
        {
            var execution = new ExecutionMetric();
            
            foreach (var step in this._Steps)
            {
                var start = DateTime.UtcNow;

                try
                {                    
                    await step.Execute();
                    var stepMetric = new StepMetric(
                            step,
                            new DateRange(start, DateTime.UtcNow));

                    execution.Add(stepMetric);
                    Console.WriteLine(stepMetric);
                }
                catch (Exception ex)
                {
                    var stepMetric = new StepMetric(
                            step,
                            new DateRange(start, DateTime.UtcNow),
                            Outcome.Failure("Error executing step", ex));

                    execution.Add(stepMetric);

                    Console.WriteLine(stepMetric);

                    return execution.Failure();
                }
            }

            return execution.Success();
        }
    }
}