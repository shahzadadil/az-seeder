namespace Msa.Seeder.Engine
{
    using System;
    using System.Collections.Generic;
    using Msa.Seeder.Core;

    public class Executor
    {
        private ICollection<Step<StepConfig>> _Steps;

        public Executor()
        {
            this._Steps = new List<Step<StepConfig>>();
        }

        public Step<StepConfig> AddStep<TStep>(String stepName)        
        {
            if (String.IsNullOrWhiteSpace(stepName))
            {
                throw new ArgumentNullException(nameof(stepName));
            }

            var step = (Step<StepConfig>)Activator.CreateInstance(typeof(TStep));
            step.Create(stepName, this._Steps.Count + 1);
            _Steps.Add(step);

            return step;
        }

        public void Execute()
        {
            foreach (var step in this._Steps)
            {
                try
                {
                    step.Execute();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error executing step. {step}", ex);
                }
            }
        }
    }
}