namespace Msa.Seeder.Engine
{
    using System;
    using System.Collections.Generic;
    using Msa.Seeder.Azure.Steps;

    public class Executor
    {
        private ICollection<Step<StepConfig>> _Steps;

        public Executor()
        {
            this._Steps = new List<Step<StepConfig>>();
        }

        public Step<StepConfig> AddStep(Step<StepConfig> stepToAdd)
        {
            if (stepToAdd == null)
            {
                throw new ArgumentNullException(nameof(stepToAdd));
            }

            _Steps.Add(stepToAdd);

            return stepToAdd;
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